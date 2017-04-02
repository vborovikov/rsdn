namespace Rsdn.Community.Interaction.Requests.Posts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Client;
    using Client.Data;
    using Relay.RequestModel;

    public class PostControl :
        IQueryHandler<ThreadPostsQuery, IEnumerable<PostDetails>>,
        ICommandHandler<MarkThreadAsViewedCommand>
    {
        private readonly IPostGateway postGateway;

        public PostControl(IPostGateway postGateway)
        {
            this.postGateway = postGateway;
        }

        public void Execute(MarkThreadAsViewedCommand command)
        {
            this.postGateway.MarkThreadAsViewed(command.ThreadId);
        }

        public IEnumerable<PostDetails> Run(ThreadPostsQuery query)
        {
            return ThreadOrganizer.Organize(this.postGateway.GetThreadPosts(query.ThreadId),
                this.postGateway.GetThread(query.ThreadId));
        }
    }
}