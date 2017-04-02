namespace Rsdn.Client.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Community.Interaction;

    public interface IForumGateway : IGateway
    {
        IEnumerable<GroupDetails> GetGroups();

        IEnumerable<ForumStatus> GetForumsStatus();

        IEnumerable<ForumDetails> GetForums();

        IEnumerable<ForumDetails> GetFavoriteForums();

        IEnumerable<ForumDetails> GetRecentForums();

        IEnumerable<ThreadDetails> GetThreads(int forumId);

        void MarkForumAsVisited(int forumId);

        void AddForumToFavorites(int forumId);

        void RemoveForumFromFavorites(int forumId);

        ForumDetails GetForum(int forumId);
    }
}