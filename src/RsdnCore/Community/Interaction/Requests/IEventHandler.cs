namespace Rsdn.Community.Interaction.Requests
{
    public interface IEventHandler<TEvent>
        where TEvent : IEvent
    {
        void Handle(TEvent e);
    }
}