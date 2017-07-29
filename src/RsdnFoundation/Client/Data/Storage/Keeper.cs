namespace Rsdn.Client.Data.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using AutoMapper;
    using Community;
    using Janus;
    using Microsoft.EntityFrameworkCore;

    internal class Keeper
    {
        private static readonly IMapper mapper;

        static Keeper()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                // Service -> Data
                config
                    .CreateMap<JanusForumGroupInfo, DbGroup>()
                    .ForMember(t => t.Id, m => m.MapFrom(s => s.forumGroupId))
                    .ForMember(t => t.Name, m => m.MapFrom(s => s.forumGroupName))
                    .ForMember(t => t.SortOrder, m => m.MapFrom(s => s.sortOrder));

                config
                    .CreateMap<JanusForumInfo, DbForum>()
                    .ForMember(t => t.Id, m => m.MapFrom(s => s.forumId))
                    .ForMember(t => t.GroupId, m => m.MapFrom(s => s.forumGroupId))
                    .ForMember(t => t.Name, m => m.MapFrom(s => s.forumName))
                    .ForMember(t => t.ShortName, m => m.MapFrom(s => s.shortForumName));

                config
                    .CreateMap<JanusMessageInfo, DbPost>()
                    .ForMember(t => t.Id, m => m.MapFrom(s => s.messageId))
                    .ForMember(t => t.ThreadId, m => m.ResolveUsing(s => s.topicId != 0 ? (int?)s.topicId : null))
                    .ForMember(t => t.SubthreadId, m => m.ResolveUsing(s => s.parentId != 0 ? (int?)s.parentId : null))
                    .ForMember(t => t.ForumId, m => m.MapFrom(s => s.forumId))
                    .ForMember(t => t.Title, m => m.MapFrom(s => s.subject))
                    .ForMember(t => t.Message, m => m.MapFrom(s => s.message))
                    .ForMember(t => t.Posted, m => m.ResolveUsing(s => s.messageDate.FromRsdnTimeToUtc()))
                    .ForMember(t => t.Updated, m =>
                        m.ResolveUsing(s => s.updateDate > DateTime.MinValue ? (DateTime?)s.updateDate.FromRsdnTimeToUtc() : null))
                    .ForMember(t => t.UserId, m => m.MapFrom(s => s.userId))
                    .ForMember(t => t.Username, m => m.MapFrom(s => s.userNick))
                    .ForMember(t => t.IsClosed, m => m.MapFrom(s => s.closed));

                config
                    .CreateMap<JanusRatingInfo, DbRating>()
                    .ForMember(t => t.ThreadId, m => m.MapFrom(s => s.topicId))
                    .ForMember(t => t.PostId, m => m.MapFrom(s => s.messageId))
                    .ForMember(t => t.UserId, m => m.MapFrom(s => s.userId))
                    .ForMember(t => t.UserFactor, m => m.MapFrom(s => s.userRating))
                    .ForMember(t => t.Rated, m => m.MapFrom(s => s.rateDate))
                    .ForMember(t => t.Value, m => m.MapFrom(s => s.rate));
            });

            mapper = mapperConfig.CreateMapper();
        }

        public Keeper()
        {
        }

        public void StoreDirectory(ForumResponse directory, CancellationToken cancelToken)
        {
            var groups = mapper.Map<IEnumerable<DbGroup>>(directory.groupList);
            var forums = mapper.Map<IEnumerable<DbForum>>(directory.forumList);

            using (var wrapper = new TransactionWrapper(cancelToken))
            {
                var db = wrapper.Connection;
                db.Groups.AddOrUpdateRange(groups, g => g.Id);
                db.Forums.AddOrUpdateRange(forums, f => f.Id);
            }
        }

        public IEnumerable<JanusMessageInfo> StoreChanges(ChangeResponse changes, CancellationToken cancelToken)
        {
            using (var wrapper = new TransactionWrapper(cancelToken))
            {
                var db = wrapper.Connection;

                StoreRatings(db, changes.newRating);

                var forumUpdates = from post in changes.newMessages
                                   group post by post.forumId into forumMessages
                                   join forum in db.Forums on forumMessages.Key equals forum.Id
                                   select new
                                   {
                                       ForumId = forum.Id,
                                       NewMessages = from msg in forumMessages
                                                     where msg.messageDate.FromRsdnTimeToUtc() > (forum.Posted ?? DateTime.MinValue)
                                                     select msg,
                                       OldMessages = from msg in forumMessages
                                                     where msg.messageDate.FromRsdnTimeToUtc() <= (forum.Posted ?? DateTime.MinValue)
                                                        && msg.updateDate > DateTime.MinValue
                                                     select msg
                                   };
                var newMsgs = forumUpdates.SelectMany(fu => fu.NewMessages).ToArray();
                var newPosts = mapper.Map<IEnumerable<DbPost>>(newMsgs);
                db.Posts.AddRange(newPosts);

                var oldMsgs = forumUpdates.SelectMany(fu => fu.OldMessages).ToArray();
                var oldPosts = mapper.Map<IEnumerable<DbPost>>(oldMsgs);
                db.Posts.UpdateRange(oldPosts);

                var orphanMsgs = changes.newMessages.Except(newMsgs).Except(oldMsgs).ToArray();
                return orphanMsgs;
            }
        }

        public void StoreArchives(TopicResponse archives, CancellationToken cancelToken)
        {
            using (var wrapper = new TransactionWrapper(cancelToken))
            {
                var db = wrapper.Connection;

                StoreRatings(db, archives.Rating);

                var arcPosts = mapper.Map<IEnumerable<DbPost>>(archives.Messages);
                db.Posts.AddOrUpdateRange(arcPosts, p => p.Id);
            }
        }

        public IEnumerable<int> StoreStats(IEnumerable<int> threadIds, CancellationToken cancelToken)
        {
            using (var wrapper = new TransactionWrapper(cancelToken))
            {
                var db = wrapper.Connection;

                // thread stats
                var missingThreadIdList = new List<int>();
                var forumIdList = new List<int>();
                foreach (var threadId in threadIds)
                {
                    var threadPost = db.Find<DbPost>(threadId);
                    if (threadPost == null)
                    {
                        missingThreadIdList.Add(threadId);
                        continue;
                    }
                    var threadReplies = db.Posts.Where(p => p.ThreadId == threadId);
                    var thread = db.Find<DbThread>(threadId) ?? new DbThread
                    {
                        ThreadId = threadId,
                    };

                    thread.PostCount = threadReplies.Count();
                    var threadViewed = (thread.Viewed ?? DateTime.MinValue);
                    thread.NewPostCount = threadReplies.Count(p => p.Posted > threadViewed);
                    db.Threads.AddOrUpdate(thread, t => t.ThreadId);

                    threadPost.Updated = threadReplies.Any() ?
                        DbPost.MostRecent(threadReplies.Max(p => p.Updated), threadReplies.Max(p => p.Posted)) :
                        threadPost.Updated ?? threadPost.Posted;
                    //db.Update(threadPost);

                    forumIdList.Add(threadPost.ForumId);
                }

                // forum stats
                var forumIds = forumIdList.Distinct().ToArray();
                foreach (var forumId in forumIds)
                {
                    var forum = db.Forums.Find(forumId);
                    var forumPosts = db.Posts.Where(p => p.ForumId == forumId);

                    forum.Fetched = DateTime.UtcNow;
                    forum.PostCount = forumPosts.Count(p => p.ThreadId == null);
                    if (forumPosts.Any())
                    {
                        forum.Posted = DbPost.MostRecent(forumPosts.Max(p => p.Updated), forumPosts.Max(p => p.Posted));
                    }
                    //db.Update(forum);
                }

                return missingThreadIdList.ToArray();
            }
        }

        public IEnumerable<DbForum> GetForums(IEnumerable<int> forumIds)
        {
            using (var connection = new RsdnDbContext())
            {
                var forums = from forum in connection.Forums
                             where forumIds.Contains(forum.Id)
                             select forum;

                return forums.ToArray();
            }
        }

        private static void StoreRatings(RsdnDbContext db, JanusRatingInfo[] ratings)
        {
            if (ratings == null || ratings.Length == 0)
                return;

            var clearRatings = from rating in ratings
                               where rating.rate == (int)VoteValue.None
                               select rating;
            foreach (var clearCmd in clearRatings)
            {
                db.Ratings
                    .Where(r => r.PostId == clearCmd.messageId && r.UserId == clearCmd.userId)
                    .Delete();
            }

            var upvotes = from rating in ratings
                          where rating.rate > 0
                          select rating;
            db.Ratings.AddOrUpdateRange(mapper.Map<IEnumerable<DbRating>>(upvotes));

            var likes = from rating in ratings
                        where rating.rate <= 0 && rating.rate != (int)VoteValue.None
                        select rating;
            db.Ratings.AddOrUpdateRange(mapper.Map<IEnumerable<DbRating>>(likes));
        }
    }
}