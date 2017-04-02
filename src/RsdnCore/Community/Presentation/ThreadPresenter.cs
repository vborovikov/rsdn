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

    public class ThreadPresenter : ModelPresenter<ThreadModel>
    {
        private readonly IPresenterHost host;
        private ItemsState postsState;
        private PostPresenter currentPost;

        public ThreadPresenter(IPresenterHost host)
        {
            this.host = host;
            this.Posts = new ObservableCollection<PostPresenter>();
        }

        public string Title => this.Model.Title;

        public string Username => this.Model.Username;

        public string Excerpt { get; private set; }

        public PostTopic Topic { get; private set; }

        public string Updated => $"📅{Whitespace.HairSpace}{this.Model.Updated.Humanize(utcDate: true)}";

        public string PostCount =>
            $"💬{Whitespace.HairSpace}{this.Model.PostCount}" + (this.Model.NewPostCount > 0 ? $"/{this.Model.NewPostCount}" : String.Empty);

        public string NewPostCount => $"💬{Whitespace.HairSpace}{this.Model.NewPostCount}";

        public string Ratings => this.Model.ToEmojiVotes();

        public bool IsNew => this.Model.IsNew;

        public ObservableCollection<PostPresenter> Posts { get; }

        public PostPresenter CurrentPost
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
            return this.host.ExecuteCommandAsync(new MarkThreadAsViewedCommand(this.Model.Id));
        }

        internal async Task LoadPostsAsync()
        {
            this.PostsState = ItemsState.Loading;
            this.PostsState = await PresenterExtensions.RefreshItemsAsync<PostPresenter, PostModel>(this.Posts,
                this.host.RunQueryAsync(new ThreadPostsQuery(this.Model.Id)),
                m => new PostPresenter(this.host) { Model = m });

            this.CurrentPost = this.Posts.FirstOrDefault();
        }

        protected override void OnModelChanged()
        {
            if (String.IsNullOrWhiteSpace(this.Model?.Excerpt))
            {
                this.Excerpt = String.Empty;
            }
            else
            {
                var markup = RsdnMarkupReference.Current.Parse(this.Model.Excerpt);

                this.Topic = ThreadOrganizer.DeterminePostTopic(markup, this.Model.Title);
                this.Excerpt = String.Join(Environment.NewLine, markup.ToString()
                    .Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries)
                    .Where(line => String.IsNullOrWhiteSpace(line) == false)
                    .Take(2));
            }

            base.OnModelChanged();
        }

        private Task UpdateAsync()
        {
            return Task.CompletedTask;
        }
    }
}