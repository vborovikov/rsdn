namespace Rsdn.Community.Interaction.Requests.Update
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Relay.RequestModel;

    public class UpdateForumCommand : CommandBase
    {
        public UpdateForumCommand(int forumId)
        {
            this.ForumId = forumId;
        }

        public int ForumId { get; }
    }
}