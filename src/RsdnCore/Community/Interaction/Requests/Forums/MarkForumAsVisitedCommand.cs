﻿namespace Rsdn.Community.Interaction.Requests.Forums
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Relay.RequestModel;

    public class MarkForumAsVisitedCommand : CommandBase
    {
        public MarkForumAsVisitedCommand(int forumId)
        {
            ForumId = forumId;
        }

        public int ForumId { get; }
    }
}