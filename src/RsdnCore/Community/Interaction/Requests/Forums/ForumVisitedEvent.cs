namespace Rsdn.Community.Interaction.Requests.Forums
{
    using Relay.RequestModel;

    public class ForumVisitedEvent : EventBase
    {
        public ForumVisitedEvent(int forumId)
        {
            ForumId = forumId;
        }

        public int ForumId { get; private set; }
    }
}