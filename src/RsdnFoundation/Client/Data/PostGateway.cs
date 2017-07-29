namespace Rsdn.Client.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Community;
    using Community.Interaction;
    using Storage;

    public class PostGateway : Gateway, IPostGateway
    {
        private static readonly IMapper mapper;

        static PostGateway()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config
                    .CreateMap<DbThread, ThreadModel>();
            });
            mapper = mapperConfig.CreateMapper();
        }

        public PostGateway()
        {
        }

        public ThreadModel GetThread(int threadId)
        {
            using (var db = new RsdnDbContext())
            {
                var thread = db.Threads.Find(threadId);
                return mapper.Map<ThreadModel>(thread);
            }
        }

        public IEnumerable<PostModel> GetThreadPosts(int threadId)
        {
            using (var db = new RsdnDbContext())
            {
                var threadPosts = from post in db.Posts
                                  where post.ThreadId == threadId || post.Id == threadId
                                  join rating in db.Ratings on post.Id equals rating.PostId into ratings
                                  orderby post.Posted ascending
                                  select new PostModel
                                  {
                                      Id = post.Id,
                                      ThreadId = post.ThreadId,
                                      SubthreadId = post.SubthreadId,
                                      Title = post.Title,
                                      Message = post.Message,
                                      Updated = post.Updated,
                                      Posted = post.Posted,
                                      Username = post.Username,
                                      InterestingCount = ratings.Count(r => r.Value == (int)VoteValue.Interesting),
                                      ThanksCount = ratings.Count(r => r.Value == (int)VoteValue.Thanks),
                                      ExcellentCount = ratings.Count(r => r.Value == (int)VoteValue.Excellent),
                                      AgreedCount = ratings.Count(r => r.Value == (int)VoteValue.Agreed),
                                      DisagreedCount = ratings.Count(r => r.Value == (int)VoteValue.Disagreed),
                                      Plus1Count = ratings.Count(r => r.Value == (int)VoteValue.Plus1),
                                      FunnyCount = ratings.Count(r => r.Value == (int)VoteValue.Funny),
                                  };

                return threadPosts.ToArray();
            }
        }

        public void MarkThreadAsViewed(int threadId)
        {
            using (var db = new RsdnDbContext())
            {
                var thread = db.Threads.Find(threadId);
                thread.Viewed = DateTime.UtcNow;
                thread.NewPostCount = 0;
                updatePolicy.Execute(() => db.SaveChanges());
            }
        }
    }
}