namespace Rsdn.Community.Interaction.Requests.Update
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Client;
    using Relay.RequestModel;

    public class UpdateControl :
        IAsyncCommandHandler<UpdateDirectoryCommand>,
        IAsyncCommandHandler<UpdateForumCommand>
    {
        private readonly IUpdateManager updateMan;

        public UpdateControl(IUpdateManager updateMan)
        {
            this.updateMan = updateMan;
        }

        public Task ExecuteAsync(UpdateForumCommand command) =>
            this.updateMan.UpdateForumsAsync(new[] { command.ForumId }, command.CancellationToken);

        public Task ExecuteAsync(UpdateDirectoryCommand command) =>
            this.updateMan.UpdateDirectoryAsync(command.CancellationToken);
    }
}