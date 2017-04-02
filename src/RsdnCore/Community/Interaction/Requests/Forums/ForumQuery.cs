namespace Rsdn.Community.Interaction.Requests.Forums
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Relay.RequestModel;

    public class ForumQuery : QueryBase<ForumModel>
    {
        public ForumQuery(int forumId)
        {
            ForumId = forumId;
        }

        public int ForumId { get; private set; }
    }
}