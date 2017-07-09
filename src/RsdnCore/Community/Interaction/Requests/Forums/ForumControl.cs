namespace Rsdn.Community.Interaction.Requests.Forums
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Client;
    using Client.Data;
    using Relay.RequestModel;

    public class ForumControl :
        IQueryHandler<GroupsQuery, IEnumerable<GroupModel>>,
        IQueryHandler<FavoriteForumsQuery, IEnumerable<ForumModel>>,
        IQueryHandler<RecentForumsQuery, IEnumerable<ForumModel>>,
        IQueryHandler<ForumThreadsQuery, IEnumerable<ThreadModel>>,
        IQueryHandler<ForumQuery, ForumModel>,
        ICommandHandler<MarkForumAsVisitedCommand>,
        ICommandHandler<AddForumToFavoritesCommand>,
        ICommandHandler<RemoveForumFromFavoritesCommand>
    {
        private readonly IEventDispatcher eventDispatcher;
        private readonly IForumGateway forumGateway;

        public ForumControl(IEventDispatcher eventDispatcher, IForumGateway forumGateway)
        {
            this.eventDispatcher = eventDispatcher;
            this.forumGateway = forumGateway;
        }

        public void Execute(AddForumToFavoritesCommand command)
        {
            this.forumGateway.AddForumToFavorites(command.ForumId);
            this.eventDispatcher.PublishAsync(new FavoritesChangedEvent(command.ForumId));
        }

        public void Execute(RemoveForumFromFavoritesCommand command)
        {
            this.forumGateway.RemoveForumFromFavorites(command.ForumId);
            this.eventDispatcher.PublishAsync(new FavoritesChangedEvent(command.ForumId, favorite: false));
        }

        public void Execute(MarkForumAsVisitedCommand command)
        {
            this.forumGateway.MarkForumAsVisited(command.ForumId);
            this.eventDispatcher.PublishAsync(new ForumVisitedEvent(command.ForumId));
        }

        public IEnumerable<GroupModel> Run(GroupsQuery query)
        {
            return this.forumGateway.GetGroups();
        }

        public ForumModel Run(ForumQuery query)
        {
            return this.forumGateway.GetForum(query.ForumId);
        }

        public IEnumerable<ForumModel> Run(RecentForumsQuery query)
        {
            return this.forumGateway.GetRecentForums();
        }

        public IEnumerable<ThreadModel> Run(ForumThreadsQuery query)
        {
            this.eventDispatcher.PublishAsync(new ForumRequestedEvent(query.ForumId));
            return ThreadOrganizer.Organize(this.forumGateway.GetThreads(query.ForumId),
                this.forumGateway.GetForum(query.ForumId));
        }

        public IEnumerable<ForumModel> Run(FavoriteForumsQuery query)
        {
            return this.forumGateway.GetFavoriteForums();
        }
    }
}