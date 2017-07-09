namespace Rsdn.Community.Interaction.Requests.Forums
{
    public class ForumRequestedEvent : EventBase
    {
        public ForumRequestedEvent(int forumId)
        {
            this.ForumId = forumId;
        }

        public int ForumId { get; }
    }
}