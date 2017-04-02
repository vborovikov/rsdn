namespace Rsdn
{
    using System;
    using System.Threading.Tasks;
    using Client.Data.Storage;
    using Community.Presentation;
    using Community.Presentation.NavigationModel;
    using Configuration;
    using Windows.ApplicationModel;
    using Windows.ApplicationModel.Activation;
    using Windows.UI;
    using Windows.UI.Core;
    using Windows.UI.ViewManagement;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;
    using Xaml;
    using Xaml.Interactivity;

    sealed partial class App : Application
    {
        public App()
        {
#if !DEBUG
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync();
#endif
            this.InitializeComponent();
            this.Suspending += HandleSuspending;

            DatabaseFactory.CreateDatabase();
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            var shell = Window.Current.Content as Shell ?? CreateShell();

            var viewModelLocator = Configure(shell.PresentationFrame);
            shell.DataContext = viewModelLocator.Shell;

            await HandleResuming(e);

            if (shell.PresentationFrame.Content == null)
            {
                shell.PresentationFrame.Navigate(typeof(DirectoryPage), null);
            }

            Window.Current.Activate();
        }

        private static Shell CreateShell()
        {
            var shell = new Shell();

            shell.PresentationFrame.NavigationFailed += HandleNavigationFailed;
            shell.PresentationFrame.Navigated += HandleNavigated;

            Window.Current.Content = shell;

            // Enable Back button
            SystemNavigationManager.GetForCurrentView().BackRequested += HandleBackRequested;
            HandleNavigated(shell.PresentationFrame, null);

            // Change title bar color
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            var brandColor = Color.FromArgb(255, 113, 187, 226);

            titleBar.BackgroundColor = titleBar.ButtonBackgroundColor = brandColor;
            titleBar.ForegroundColor = titleBar.ButtonForegroundColor = Colors.White;

            return shell;
        }

        private static void HandleBackRequested(object sender, BackRequestedEventArgs e)
        {
            var rootFrame = ((Shell)Window.Current.Content).PresentationFrame;

            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
                e.Handled = true;
            }
        }

        private static void HandleNavigated(object sender, NavigationEventArgs e)
        {
            // Each time a navigation event occurs, update the Back button's visibility
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                ((Frame)sender).CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
        }

        private static void HandleNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private async static Task HandleResuming(LaunchActivatedEventArgs e)
        {
            if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                await SuspensionManager.RestoreAsync();
            }

            var shell = Window.Current.Content as Shell;
            if (shell != null)
            {
                var shellPresenter = shell.DataContext as ShellViewModel;
                await (shellPresenter as ITombstone)?.OnDeserializingAsync(SuspensionManager.SessionState);
            }
        }

        private async void HandleSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            try
            {
                var shell = Window.Current.Content as Shell;
                if (shell != null)
                {
                    var shellPresenter = shell.DataContext as ShellViewModel;
                    await (shellPresenter as ITombstone)?.OnSerializingAsync(SuspensionManager.SessionState);
                }

                await SuspensionManager.SaveAsync();
            }
            finally
            {
                deferral.Complete();
            }
        }

        private PresenterLocator Configure(Frame presentationFrame)
        {
            var serviceProvider = ObjectGraph.Build(presentationFrame);

            if (this.Resources.ContainsKey(nameof(PresenterLocator)))
            {
                this.Resources.Remove(nameof(PresenterLocator));
            }
            var presenterLocator = serviceProvider.GetService(typeof(PresenterLocator)) as PresenterLocator;
            this.Resources.Add(nameof(PresenterLocator), presenterLocator);
            return presenterLocator;
        }
    }
}