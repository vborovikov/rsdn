namespace Rsdn.Community.Presentation
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Client;
    using Humanizer;
    using Interaction;
    using Interaction.Requests.Posts;
    using Markup;
    using Text;
    using IUwpCommand = System.Windows.Input.ICommand;

    public class ThreadViewModel : DataViewModel<ThreadDetails>
    {
        private readonly IPresenterHost host;
        private ItemsState postsState;
        private PostViewModel currentPost;

        public ThreadViewModel(IPresenterHost host)
        {
            this.host = host;
            this.Posts = new ObservableCollection<PostViewModel>();
        }

        public string Title => this.Data.Title;

        public string Username => this.Data.Username;

        public string Excerpt { get; private set; }

        public PostTopic Topic { get; private set; }

        public string Updated => $"📅{Whitespace.HairSpace}{this.Data.Updated.Humanize(utcDate: true)}";

        public string PostCount =>
            $"💬{Whitespace.HairSpace}{this.Data.PostCount}" + (this.Data.NewPostCount > 0 ? $"/{this.Data.NewPostCount}" : String.Empty);

        public string NewPostCount => $"💬{Whitespace.HairSpace}{this.Data.NewPostCount}";

        public string Ratings => this.Data.ToEmojiVotes();

        public bool IsNew => this.Data.IsNew;

        public ObservableCollection<PostViewModel> Posts { get; }

        public PostViewModel CurrentPost
        {
            get
            {
                return this.currentPost;
            }

            set
            {
                this.currentPost = value;
                RaisePropertyChanged(nameof(this.CurrentPost));
            }
        }

        public ItemsState PostsState
        {
            get
            {
                return this.postsState;
            }

            set
            {
                if (this.postsState != value)
                {
                    this.postsState = value;
                    RaisePropertyChanged(nameof(this.PostsState));
                }
            }
        }

        public IUwpCommand UpdateCommand => GetCommand(UpdateAsync);

        internal Task MarkAsViewedAsync()
        {
            return this.host.ExecuteCommandAsync(new MarkThreadAsViewedCommand(this.Data.Id));
        }

        internal async Task LoadPostsAsync()
        {
            this.PostsState = ItemsState.Loading;
            this.PostsState = await PresenterExtensions.RefreshItemsAsync<PostViewModel, PostDetails>(this.Posts,
                this.host.RunQueryAsync(new ThreadPostsQuery(this.Data.Id)),
                m => new PostViewModel(this.host) { Data = m });

            this.CurrentPost = this.Posts.FirstOrDefault();
        }

        protected override void OnDataChanged()
        {
            if (String.IsNullOrWhiteSpace(this.Data?.Excerpt))
            {
                this.Excerpt = String.Empty;
            }
            else
            {
                var markup = RsdnMarkupReference.Current.Parse(this.Data.Excerpt);

                this.Topic = ThreadOrganizer.DeterminePostTopic(markup, this.Data.Title);
                this.Excerpt = String.Join(Environment.NewLine, markup.ToString()
                    .Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries)
                    .Where(line => String.IsNullOrWhiteSpace(line) == false)
                    .Take(2));
            }

            base.OnDataChanged();
        }

        private Task UpdateAsync()
        {
            return Task.CompletedTask;
        }
    }
}