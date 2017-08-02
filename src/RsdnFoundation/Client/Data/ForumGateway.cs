namespace Rsdn.Client.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Community;
    using Community.Interaction;
    using Storage;

    public class ForumGateway : Gateway, IForumGateway
    {
        private const int MaxSidebarForumCount = 6;
        private static readonly IMapper mapper;
        private readonly ICredentialManager credentialMan;

        static ForumGateway()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config
                    .CreateMap<DbForum, ForumModel>();

                config
                    .CreateMap<DbPost, ThreadModel>();
            });
            mapper = mapperConfig.CreateMapper();
        }

        public ForumGateway(ICredentialManager credentialMan)
        {
            this.credentialMan = credentialMan;
        }

        public IEnumerable<ForumModel> GetForums()
        {
            using (var db = new RsdnDbContext())
            {
                return mapper.Map<IEnumerable<ForumModel>>(db.Forums);
            }
        }

        public IEnumerable<ForumModel> GetFavoriteForums()
        {
            using (var db = new RsdnDbContext())
            {
                IQueryable<DbForum> forums = from forum in db.Forums
                                             where forum.IsFavorite
                                             orderby forum.Visited descending
                                             select forum;
                forums = forums.Take(MaxSidebarForumCount);

                return mapper.Map<IEnumerable<ForumModel>>(forums.ToArray());
            }
        }

        public IEnumerable<ForumModel> GetRecentForums()
        {
            using (var db = new RsdnDbContext())
            {
                IQueryable<DbForum> forums = from forum in db.Forums
                                             where forum.Visited != null && forum.IsFavorite == false
                                             orderby forum.Visited descending
                                             select forum;
                forums = forums.Take(MaxSidebarForumCount);

                return mapper.Map<IEnumerable<ForumModel>>(forums.ToArray());
            }
        }

        public IEnumerable<ForumStatus> GetForumsStatus()
        {
            using (var db = new RsdnDbContext())
            {
                var forumsStatus = from forum in db.Forums
                                   select new ForumStatus(forum.Id, forum.IsFavorite,
                                        forum.Fetched, forum.Visited,
                                        forum.Posted, forum.PostCount);

                return forumsStatus.ToArray();
            }
        }

        public IEnumerable<ThreadModel> GetThreads(int forumId)
        {
            using (var db = new RsdnDbContext())
            {
                var threads = from post in db.Posts
                              where post.ThreadId == null && post.ForumId == forumId
                              join rating in db.Ratings on post.Id equals rating.ThreadId into ratings
                              join thread in db.Threads on post.Id equals thread.ThreadId
                              orderby post.Updated descending, post.Posted descending
                              select post.CreateThreadModel(thread, ratings, this.credentialMan.User.Id, null);

                return threads.ToArray();
            }
        }

        public void MarkForumAsVisited(int forumId)
        {
            using (var db = new RsdnDbContext())
            {
                var forum = db.Forums.Find(forumId);
                forum.Visited = DateTime.UtcNow;
                updatePolicy.Execute(() => db.SaveChanges());
            }
        }

        public void AddForumToFavorites(int forumId)
        {
            ChangeFavorite(forumId, true);
        }

        public void RemoveForumFromFavorites(int forumId)
        {
            ChangeFavorite(forumId, false);
        }

        public IEnumerable<GroupModel> GetGroups()
        {
            using (var db = new RsdnDbContext())
            {
                var groups = db.Groups.ToArray();
                var forums = db.Forums.ToArray();

                var forumsInGroups = from g in groups
                                     join f in forums on g.Id equals f.GroupId into groupForums
                                     orderby g.SortOrder
                                     select new GroupModel
                                     {
                                         Id = g.Id,
                                         Name = g.Name,
                                         SortOrder = g.SortOrder,
                                         Forums = mapper.Map<IEnumerable<DbForum>, IEnumerable<ForumModel>>(
                                             from gf in groupForums
                                             orderby gf.Name
                                             select gf)
                                     };

                return forumsInGroups.ToArray();
            }
        }

        public ForumModel GetForum(int forumId)
        {
            using (var db = new RsdnDbContext())
            {
                var forum = db.Forums.Find(forumId);
                return mapper.Map<ForumModel>(forum);
            }
        }

        private void ChangeFavorite(int forumId, bool flag)
        {
            using (var db = new RsdnDbContext())
            {
                var forum = db.Forums.Find(forumId);
                forum.IsFavorite = flag;
                db.SaveChanges();
            }
        }
    }
}