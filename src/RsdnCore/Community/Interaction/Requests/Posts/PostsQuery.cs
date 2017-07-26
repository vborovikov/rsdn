namespace Rsdn.Community.Interaction.Requests.Posts
{
    using System.Collections.Generic;

    public class PostsQuery : QueryBase<IEnumerable<ThreadModel>>
    {
        public PostsQuery(int userId)
        {
            this.UserId = userId;
        }

        public int UserId { get; }
    }
}