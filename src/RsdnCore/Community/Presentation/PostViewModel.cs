﻿namespace Rsdn.Community.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Client;
    using Humanizer;
    using Interaction;
    using Interaction.Requests.Posts;

    public class PostViewModel : DataViewModel<PostDetails>
    {
        private readonly IPresenterHost host;

        private ReplyMode replyMode;
        private string replyTitle;
        private string replyMessage;

        public PostViewModel(IPresenterHost host)
        {
            this.host = host;
            this.replyMode = ReplyMode.Inactive;
        }

        public string Title => this.Data.Title;

        public string Message => this.Data.Message;

        public string Username => this.Data.Username;

        public string Posted => this.Data.Posted.Humanize(utcDate: true);

        public string Ratings => this.Data.ToEmojiVotes(expanded: true);

        public int Level => this.Data.Level;

        public bool IsNew => this.Data.IsNew;

        public ReplyMode ReplyMode
        {
            get
            {
                return this.replyMode;
            }

            set
            {
                if (this.replyMode != value)
                {
                    this.replyMode = value;
                    RaisePropertyChanged(nameof(this.ReplyMode));
                    RaisePropertyChanged(nameof(this.IsReplyingTo));
                }
            }
        }

        public bool IsReplyingTo => this.replyMode == ReplyMode.Active;

        public string ReplyTitle
        {
            get
            {
                return this.replyTitle;
            }

            set
            {
                this.replyTitle = value;
                RaisePropertyChanged(nameof(this.ReplyTitle));
            }
        }

        public string ReplyMessage
        {
            get
            {
                return this.replyMessage;
            }

            set
            {
                this.replyMessage = value;
                RaisePropertyChanged(nameof(this.ReplyMessage));
            }
        }

        public ICommand ReplyCommand => GetCommand(Reply, CanReply);

        public ICommand CommitReplyCommand => GetCommand(CommitReply, CanCommitReply);

        public ICommand CancelReplyCommand => GetCommand(CancelReply, CanCancelReply);

        public ICommand VoteCommand => GetCommand<string>(Vote, CanVote);

        private bool CanCommitReply()
        {
            return String.IsNullOrWhiteSpace(this.ReplyTitle) == false &&
                String.IsNullOrWhiteSpace(this.ReplyMessage) == false;
        }

        private async Task CommitReply()
        {
            using (Busy())
            {
                var replyPostCommand = new NewReplyCommand(this.Data.Id, this.ReplyTitle, this.ReplyMessage);
                ClearReply();
                await this.host.ExecuteCommandAsync(replyPostCommand);
            }
        }

        private bool CanCancelReply()
        {
            return this.ReplyMode == ReplyMode.Active;
        }

        private Task CancelReply()
        {
            return ClearReply();
        }

        private Task ClearReply()
        {
            this.ReplyMode = ReplyMode.Inactive;
            this.ReplyTitle = null;
            this.ReplyMessage = null;
            return Task.CompletedTask;
        }

        private bool CanReply()
        {
            return this.ReplyMode == ReplyMode.Inactive;
        }

        private Task Reply()
        {
            this.ReplyMode = ReplyMode.Active;
            this.ReplyTitle = ThreadOrganizer.GetReplyTitle(this.Data.Title);
            this.ReplyMessage = ThreadOrganizer.GetReplyMessage(this.Data);
            return Task.CompletedTask;
        }

        private bool CanVote(string str)
        {
            VoteValue rate;
            return Enum.TryParse(str, out rate);
        }

        private async Task Vote(string str)
        {
            using (Busy())
            {
                await this.host.ExecuteCommandAsync(new VotePostCommand(this.Data.Id,
                    (VoteValue)Enum.Parse(typeof(VoteValue), str)));
            }
        }
    }
}