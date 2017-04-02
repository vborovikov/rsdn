namespace Rsdn.Community.Interaction.Requests
{
    using Relay.RequestModel;

    public abstract class CommandBase : RequestBase, ICommand
    {
        protected CommandBase()
        {
        }
    }
}