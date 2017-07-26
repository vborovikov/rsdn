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

    public class PostGateway : Gateway, IPostGateway
    {
        private static readonly IMapper mapper;
        private static readonly Policy updatePolicy;
        private readonly IDatabaseFactory databaseFactory;

        static PostGateway()
        {
            updatePolicy = Policy
                .Handle<SQLiteException>()
                .WaitAndRetry(2, r => TimeSpan.FromSeconds(1));

            var mapperConfig = new MapperConfiguration(config =>
            {
                config
                    .CreateMap<DbThread, ThreadModel>();
            });
            mapper = mapperConfig.CreateMapper();
        }

        public PostGateway(IDatabaseFactory databaseFactory)
        {
            this.databaseFactory = databaseFactory;
        }

        public ThreadModel GetThread(int threadId)
        {
            using (var db = this.databaseFactory.GetDatabase())
            {
                var thread = db.Get<DbThread>(threadId);
                return mapper.Map<ThreadModel>(thread);
            }
        }

        public IEnumerable<PostModel> GetThreadPosts(int threadId)
        {
            using (var db = this.databaseFactory.GetDatabase())
            {
                var threadPosts = db.Query<PostModel>(
                    "select Post.*, Ratings.* " +
                    "from Post " +
                    RatingsJoin +
                    "where Post.ThreadId = ? or Post.Id = ? " +
                    "order by Post.Posted asc ",
                    threadId, threadId);

                return threadPosts;
            }
        }

        public IEnumerable<ThreadModel> GetUserPosts(int userId)
        {
            using (var db = this.databaseFactory.GetDatabase())
            {
                //todo: get replies
                //todo: get posts

                //var posts = db.Query<ThreadModel>(
                //    ThreadsSelect +
                //    //fixme: how to join original thread posts?
                //    "join Thread on Post.ThreadId = Thread.ThreadId " +
                //    RatingsJoin +
                //    "where Post.UserId = ? " +
                //    "order by Post.Updated desc, Post.Posted desc", userId);

                return Enumerable.Empty<ThreadModel>();
            }
        }

        public void MarkThreadAsViewed(int threadId)
        {
            using (var db = this.databaseFactory.GetDatabase())
            {
                var thread = db.Get<DbThread>(threadId);
                thread.Viewed = DateTime.UtcNow;
                thread.NewPostCount = 0;
                updatePolicy.Execute(() => db.Update(thread));
            }
        }
    }
}