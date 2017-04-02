namespace Rsdn.Community.Interaction.Requests
{
    using System.Threading;
    using Relay.RequestModel;

    public abstract class RequestBase : IRequest
    {
        protected RequestBase()
        {
            this.CancellationToken = CancellationToken.None;
        }

        public CancellationToken CancellationToken { get; set; }
    }
}