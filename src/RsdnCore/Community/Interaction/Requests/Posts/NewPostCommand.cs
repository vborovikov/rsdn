namespace Rsdn.Community.Interaction.Requests.Posts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Relay.RequestModel;

    public abstract class NewPostCommand : CommandBase
    {
        protected NewPostCommand(string title, string message)
        {
            this.Message = message;
            this.Title = title;
        }

        public string Title { get; private set; }

        public string Message { get; private set; }
    }

    public class NewReplyCommand : NewPostCommand
    {
        public NewReplyCommand(int postId, string title, string message) : base(title, message)
        {
            this.PostId = postId;
        }

        public int PostId { get; private set; }
    }

    public class NewThreadCommand : NewPostCommand
    {
        public NewThreadCommand(int forumId, string title, string message) : base(title, message)
        {
            this.ForumId = forumId;
        }

        public int ForumId { get; private set; }
    }
}