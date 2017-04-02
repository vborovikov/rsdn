namespace Rsdn.Client.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Community.Interaction;

    public interface IForumGateway : IGateway
    {
        IEnumerable<GroupModel> GetGroups();

        IEnumerable<ForumStatus> GetForumsStatus();

        IEnumerable<ForumModel> GetForums();

        IEnumerable<ForumModel> GetFavoriteForums();

        IEnumerable<ForumModel> GetRecentForums();

        IEnumerable<ThreadModel> GetThreads(int forumId);

        void MarkForumAsVisited(int forumId);

        void AddForumToFavorites(int forumId);

        void RemoveForumFromFavorites(int forumId);

        ForumModel GetForum(int forumId);
    }
}