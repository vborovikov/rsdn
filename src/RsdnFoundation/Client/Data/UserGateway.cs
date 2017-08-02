namespace Rsdn.Client.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Community.Interaction;
    using Storage;

    public class UserGateway : IUserGateway
    {
        private readonly ICredentialManager credentialMan;

        public UserGateway(ICredentialManager credentialMan)
        {
            this.credentialMan = credentialMan;
        }

        public IEnumerable<ThreadModel> GetActivity(int userId)
        {
            using (var db = new RsdnDbContext())
            {
                var posts = from post in db.Posts
                            join rating in db.Ratings on post.Id equals rating.PostId into ratings
                            join parent in db.Posts on post.SubthreadId equals parent.Id into parents
                            join thread in db.Threads on post.ThreadId equals thread.ThreadId
                            orderby post.Updated descending, post.Posted descending
                            from parent in parents.DefaultIfEmpty()
                            where post.UserId == userId || parent.UserId == userId
                            select post.CreateThreadModel(thread, ratings, userId, parent);

                return posts.ToArray();
            }
        }
    }
}