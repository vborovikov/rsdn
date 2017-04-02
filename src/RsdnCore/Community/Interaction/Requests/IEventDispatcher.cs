namespace Rsdn.Community.Interaction.Requests
{
    using System.Threading.Tasks;

    public interface IEventDispatcher
    {
        Task PublishAsync<TEvent>(TEvent e) where TEvent : IEvent;
    }
}