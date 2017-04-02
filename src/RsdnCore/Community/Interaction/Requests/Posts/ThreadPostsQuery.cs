namespace Rsdn.Community.Interaction.Requests.Posts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Relay.RequestModel;

    public class ThreadPostsQuery : QueryBase<IEnumerable<PostModel>>
    {
        public ThreadPostsQuery(int threadId)
        {
            this.ThreadId = threadId;
        }

        public int ThreadId { get; private set; }
    }
}