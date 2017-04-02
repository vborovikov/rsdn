namespace Rsdn.Community.Interaction.Requests
{
    using Relay.RequestModel;

    public abstract class QueryBase<TResult> : RequestBase, IQuery<TResult>
    {
        protected QueryBase()
        {
        }
    }
}