namespace Rsdn.Client.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Community.Interaction;
    using Polly;
    using SQLite;
    using Storage;

    public class ForumGateway : Gateway, IForumGateway
    {
        private const int MaxSidebarForumCount = 6;
        private static readonly IMapper mapper;
        private static readonly Policy updatePolicy;
        private readonly IDatabaseFactory databaseFactory;

        static ForumGateway()
        {
            updatePolicy = Policy
                .Handle<SQLiteException>()
                .WaitAndRetry(2, r => TimeSpan.FromSeconds(1));

            var mapperConfig = new MapperConfiguration(config =>
            {
                config
                    .CreateMap<DbForum, ForumModel>();

                config
                    .CreateMap<DbPost, ThreadModel>();
            });
            mapper = mapperConfig.CreateMapper();
        }

        public ForumGateway(IDatabaseFactory databaseFactory)
        {
            this.databaseFactory = databaseFactory;
        }

        public IEnumerable<ForumModel> GetForums()
        {
            using (var db = this.databaseFactory.GetDatabase())
            {
                return mapper.Map<IEnumerable<ForumModel>>(db.Table<DbForum>());
            }
        }

        public IEnumerable<ForumModel> GetFavoriteForums()
        {
            using (var db = this.databaseFactory.GetDatabase())
            {
                var forums = from forum in db.Table<DbForum>()
                             where forum.IsFavorite
                             orderby forum.Visited descending
                             select forum;
                forums = forums.Take(MaxSidebarForumCount);

                return mapper.Map<IEnumerable<ForumModel>>(forums.ToArray());
            }
        }

        public IEnumerable<ForumModel> GetRecentForums()
        {
            using (var db = this.databaseFactory.GetDatabase())
            {
                var forums = from forum in db.Table<DbForum>()
                             orderby forum.Visited descending
                             where forum.Visited != null && forum.IsFavorite == false
                             select forum;
                forums = forums.Take(MaxSidebarForumCount);

                return mapper.Map<IEnumerable<ForumModel>>(forums.ToArray());
            }
        }

        public IEnumerable<ForumStatus> GetForumsStatus()
        {
            using (var db = this.databaseFactory.GetDatabase())
            {
                var forumsStatus = from forum in db.Table<DbForum>()
                                   select new ForumStatus(forum.Id, forum.IsFavorite,
                                        forum.Fetched, forum.Visited,
                                        forum.Posted, forum.PostCount);

                return forumsStatus.ToArray();
            }
        }

        public IEnumerable<ThreadModel> GetThreads(int forumId)
        {
            using (var db = this.databaseFactory.GetDatabase())
            {
                var details = db.Query<ThreadModel>(
                    ThreadsSelect +
                    "join Thread on Post.Id = Thread.ThreadId " +
                    RatingsJoin +
                    "where Post.ThreadId is null and Post.ForumId = ? " +
                    "order by Post.Updated desc, Post.Posted desc", forumId);

                return details;
            }
        }

        public void MarkForumAsVisited(int forumId)
        {
            using (var db = this.databaseFactory.GetDatabase())
            {
                var forum = db.Get<DbForum>(forumId);
                forum.Visited = DateTime.UtcNow;
                updatePolicy.Execute(() => db.Update(forum));
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
            using (var db = this.databaseFactory.GetDatabase())
            {
                var groups = db.Table<DbGroup>().ToArray();
                var forums = db.Table<DbForum>().ToArray();

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
            using (var db = this.databaseFactory.GetDatabase())
            {
                var forum = db.Get<DbForum>(forumId);
                return mapper.Map<ForumModel>(forum);
            }
        }

        private void ChangeFavorite(int forumId, bool flag)
        {
            using (var db = this.databaseFactory.GetDatabase())
            {
                var forum = db.Get<DbForum>(forumId);
                forum.IsFavorite = flag;
                db.Update(forum);
            }
        }
    }
}