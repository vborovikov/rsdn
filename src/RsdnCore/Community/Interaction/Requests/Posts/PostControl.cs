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
        IQueryHandler<ThreadPostsQuery, IEnumerable<PostModel>>,
        ICommandHandler<MarkThreadAsViewedCommand>,
        IQueryHandler<PostsQuery, IEnumerable<ThreadModel>>
    {
        private readonly IPostGateway postGateway;
        private readonly IUserGateway userGateway;
        private readonly ICredentialManager credentialMan;

        public PostControl(IPostGateway postGateway, IUserGateway userGateway, ICredentialManager credentialMan)
        {
            this.postGateway = postGateway;
            this.userGateway = userGateway;
            this.credentialMan = credentialMan;
        }

        public void Execute(MarkThreadAsViewedCommand command)
        {
            this.postGateway.MarkThreadAsViewed(command.ThreadId);
        }

        public IEnumerable<ThreadModel> Run(PostsQuery query)
        {
            return this.userGateway.GetActivity(this.credentialMan.User.Id);
        }

        public IEnumerable<PostModel> Run(ThreadPostsQuery query)
        {
            return ThreadOrganizer.Organize(this.postGateway.GetThreadPosts(query.ThreadId),
                this.postGateway.GetThread(query.ThreadId));
        }
    }
}