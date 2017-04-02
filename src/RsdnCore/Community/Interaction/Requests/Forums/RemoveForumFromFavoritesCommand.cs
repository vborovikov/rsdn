namespace Rsdn.Community.Interaction.Requests.Forums
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Relay.RequestModel;

    public class RemoveForumFromFavoritesCommand : CommandBase
    {
        public RemoveForumFromFavoritesCommand(int forumId)
        {
            this.ForumId = forumId;
        }

        public int ForumId { get; }
    }
}