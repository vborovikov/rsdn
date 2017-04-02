namespace Rsdn.Community.Interaction.Requests.Forums
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Relay.RequestModel;

    public class ForumThreadsQuery : QueryBase<IEnumerable<ThreadDetails>>
    {
        public ForumThreadsQuery(int forumId)
        {
            this.ForumId = forumId;
        }

        public int ForumId { get; }
    }
}