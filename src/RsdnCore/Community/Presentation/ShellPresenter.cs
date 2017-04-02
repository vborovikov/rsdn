namespace Rsdn.Community.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using System.Threading.Tasks;
    using Interaction;
    using Interaction.Requests;
    using Interaction.Requests.Credentials;
    using Interaction.Requests.Forums;
    using NavigationModel;
    using Relay.RequestModel;
    using IUwpCommand = System.Windows.Input.ICommand;

    public class ShellPresenter : NavigablePresenterHost,
        IAsyncEventHandler<FavoritesChangedEvent>,
        IAsyncEventHandler<ForumVisitedEvent>
    {
        private ObservableCollection<ForumHub> forumHubs;

        public ShellPresenter(INavigationService navigationService, IRequestDispatcher requestDispatcher, IDialogManager dialogManager)
            : base(navigationService, requestDispatcher, dialogManager)
        {
            this.forumHubs = new ObservableCollection<ForumHub>();
        }

        public ObservableCollection<ForumHub> ForumHubs => this.forumHubs;

        public IUwpCommand ForumCommand => GetCommand<ForumModel>(OpenForum);

        public IUwpCommand VotesCommand => GetCommand(OpenVotes);

        public IUwpCommand PostsCommand => GetCommand(OpenPosts);

        public IUwpCommand DirectoryCommand => GetCommand(OpenDirectory);

        public IUwpCommand SettingsCommand => GetCommand(OpenSettings);

        Task IAsyncEventHandler<FavoritesChangedEvent>.HandleAsync(FavoritesChangedEvent e) =>
            LoadHubsAsync();

        Task IAsyncEventHandler<ForumVisitedEvent>.HandleAsync(ForumVisitedEvent e) =>
            LoadHubsAsync();

        internal async Task LoadHubsAsync()
        {
            using (this.Busy())
            {
                this.forumHubs.Clear();

                this.forumHubs.Add(new ForumHub("Favorites", await RunQueryAsync(new FavoriteForumsQuery()))
                {
                    Tag = "Favorite"
                });
                this.forumHubs.Add(new ForumHub("Recent", await RunQueryAsync(new RecentForumsQuery()))
                {
                    Tag = "Clock"
                });
            }
        }

        protected override async Task OnDeserializingAsync(IDictionary<string, object> state)
        {
            await LoadHubsAsync();
            await VerifyCredentialAsync();
        }

        private async Task<CredentialVerificationResult> VerifyCredentialAsync()
        {
            var credential = await RunQueryAsync(new CredentialQuery());
            var verifyResult = await RunQueryAsync(new VerifyCredentialQuery(credential));
            while (verifyResult != CredentialVerificationResult.Success)
            {
                await ShowMessage(verifyResult.ToString());
                credential.Password = String.Empty;
                var canSignin = await ShowDialog(
                    typeof(SigninPresenter).GetTypeInfo().GetCustomAttribute<DialogAttribute>().DialogType,
                    credential);
                if (canSignin ?? false)
                    await ExecuteCommandAsync(new SigninCommand(credential));
                else
                    break;
                verifyResult = await RunQueryAsync(new VerifyCredentialQuery(credential));
            }

            return verifyResult;
        }

        private Task OpenForum(ForumModel forum)
        {
            this.navigationService.Navigate(typeof(ForumPresenter), parameter: forum.Id);
            return Task.CompletedTask;
        }

        private Task OpenVotes()
        {
            this.navigationService.Navigate(typeof(VotesPresenter));
            return Task.CompletedTask;
        }

        private Task OpenPosts()
        {
            this.navigationService.Navigate(typeof(PostsPresenter));
            return Task.CompletedTask;
        }

        private Task OpenDirectory()
        {
            this.navigationService.Navigate(typeof(DirectoryPresenter));
            while (this.navigationService.CanGoBack)
            {
                this.navigationService.RemoveBackEntry();
            }
            return Task.CompletedTask;
        }

        private Task OpenSettings()
        {
            return this.dialogManager.ShowMessage("Settings");
        }
    }
}