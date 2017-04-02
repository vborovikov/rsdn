namespace Rsdn.Community.Interaction.Requests.Posts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Community;
    using Relay.RequestModel;

    public class VotePostCommand : CommandBase
    {
        public VotePostCommand(int postId, VoteValue vote)
        {
            Vote = vote;
            PostId = postId;
        }

        public int PostId { get; }

        public VoteValue Vote { get; }
    }
}