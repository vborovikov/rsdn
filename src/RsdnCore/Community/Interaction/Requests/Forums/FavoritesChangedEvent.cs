namespace Rsdn.Community.Interaction.Requests.Forums
{
    using Relay.RequestModel;

    public class FavoritesChangedEvent : EventBase
    {
        public FavoritesChangedEvent(int forumId, bool favorite = true)
        {
            this.ForumId = forumId;
            this.ForumIsFavorite = favorite;
        }

        public int ForumId { get; }

        public bool ForumIsFavorite { get; }
    }
}