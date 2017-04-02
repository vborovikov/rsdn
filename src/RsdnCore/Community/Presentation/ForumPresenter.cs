namespace Rsdn.Community.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Interaction;
    using Interaction.Requests.Forums;
    using Interaction.Requests.Update;
    using NavigationModel;
    using Relay.RequestModel;
    using IUwpCommand = System.Windows.Input.ICommand;

    [Navigable("Rsdn.Xaml.ForumPage, Rsdn")]
    public class ForumPresenter : ActivityPresenter
    {
        private ForumModel data;

        public ForumPresenter(IPresenterHost host)
            : base(host)
        {
            this.data = ForumModel.Empty;
        }

        public int Id => this.data.Id;

        public override string Name => this.data.Name;

        public bool IsFavorite => this.data.IsFavorite;

        public IUwpCommand UpdateCommand => GetCommand(UpdateAsync);

        public IUwpCommand ToggleFavoriteCommand => GetCommand(ToggleFavoriteAsync);

        protected override Task OnSerializingAsync(IDictionary<string, object> state)
        {
            state[String.Empty] = this.data.Id;
            return Task.CompletedTask;
        }

        protected override async Task OnNavigatedFromAsync(object parameter)
        {
            await MarkAsVisitedAsync();
            await base.OnNavigatedFromAsync(parameter);
        }

        protected override async Task OnDeserializingAsync(IDictionary<string, object> state)
        {
            object dataObj;
            if (state.TryGetValue(String.Empty, out dataObj))
            {
                using (Busy())
                {
                    var forumId = (int)dataObj;
                    this.data = await this.host.RunQueryAsync(new ForumQuery(forumId));
                    RaisePropertyChanged();
                    await LoadThreadsAsync();
                }
            }
        }

        protected override IQuery<IEnumerable<ThreadModel>> GetThreadsQuery()
        {
            return new ForumThreadsQuery(this.data.Id);
        }

        private Task MarkAsVisitedAsync()
        {
            return this.host.ExecuteCommandAsync(new MarkForumAsVisitedCommand(this.data.Id));
        }

        private async Task ToggleFavoriteAsync()
        {
            using (Busy())
            {
                if (this.data.IsFavorite)
                {
                    await this.host.ExecuteCommandAsync(new RemoveForumFromFavoritesCommand(this.data.Id));
                }
                else
                {
                    await this.host.ExecuteCommandAsync(new AddForumToFavoritesCommand(this.data.Id));
                }

                this.data.IsFavorite = !this.data.IsFavorite;
                RaisePropertyChanged(nameof(this.IsFavorite));
            }
        }

        private async Task UpdateAsync()
        {
            using (this.Busy())
            {
                await this.host.ExecuteCommandAsync(new UpdateForumCommand(this.data.Id));
                await LoadThreadsAsync();
            }
        }
    }
}