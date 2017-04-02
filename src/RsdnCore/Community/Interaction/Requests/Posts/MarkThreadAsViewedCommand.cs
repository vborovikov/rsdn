namespace Rsdn.Community.Interaction.Requests.Posts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Relay.RequestModel;

    public class MarkThreadAsViewedCommand : CommandBase
    {
        public MarkThreadAsViewedCommand(int threadId)
        {
            ThreadId = threadId;
        }

        public int ThreadId { get; }
    }
}