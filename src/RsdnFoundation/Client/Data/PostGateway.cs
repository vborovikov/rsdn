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
                    .CreateMap<DbThread, ThreadDetails>();
            });
            mapper = mapperConfig.CreateMapper();
        }

        public PostGateway(IDatabaseFactory databaseFactory)
        {
            this.databaseFactory = databaseFactory;
        }

        public ThreadDetails GetThread(int threadId)
        {
            using (var db = this.databaseFactory.GetDatabase())
            {
                var thread = db.Get<DbThread>(threadId);
                return mapper.Map<ThreadDetails>(thread);
            }
        }

        public IEnumerable<PostDetails> GetThreadPosts(int threadId)
        {
            using (var db = this.databaseFactory.GetDatabase())
            {
                var threadPosts = db.Query<PostDetails>(
                    "select Post.*, Ratings.* " +
                    "from Post " +
                    RatingsJoin +
                    "where Post.ThreadId = ? or Post.Id = ? " +
                    "order by Post.Posted asc ",
                    threadId, threadId);

                return threadPosts;
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