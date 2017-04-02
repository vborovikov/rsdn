namespace Rsdn.Community.Presentation.NavigationModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Relay.PresentationModel;
    using Relay.RequestModel;

    public abstract class NavigablePresenterHost : NavigablePresenter, IPresenterHost
    {
        protected readonly INavigationService navigationService;
        protected readonly IRequestDispatcher requestDispatcher;
        protected readonly IDialogManager dialogManager;

        protected NavigablePresenterHost(INavigationService navigationService, IRequestDispatcher requestDispatcher, IDialogManager dialogManager)
        {
            this.navigationService = navigationService;
            this.requestDispatcher = requestDispatcher;
            this.dialogManager = dialogManager;
        }

        Task IPresenterHost.ExecuteCommandAsync(ICommand command) => ExecuteCommandAsync(command);

        Task<T> IPresenterHost.RunQueryAsync<T>(IQuery<T> query) => RunQueryAsync(query);

        Task IPresenterHost.ShowMessage(string content) => ShowMessage(content);

        Task<bool?> IPresenterHost.ShowDialog(string dialogType, object dialogModel) => ShowDialog(dialogType, dialogModel);

        public void Navigate<TPresenter>(object parameter) where TPresenter : Presenter
        {
            this.navigationService.Navigate(typeof(TPresenter), parameter: parameter);
        }

        protected async Task ExecuteCommandAsync(ICommand command)
        {
            try
            {
                await this.requestDispatcher.ExecuteNonGenericAsync(command);
            }
            catch (Exception x)
            {
                await HandleExceptionAsync(x);
            }
        }

        protected async Task<T> RunQueryAsync<T>(IQuery<T> query)
        {
            try
            {
                return await this.requestDispatcher.RunAsync(query);
            }
            catch (Exception x)
            {
                await HandleExceptionAsync(x);
                return default(T);
            }
        }

        protected Task ShowMessage(string content)
        {
            return this.dialogManager.ShowMessage(content);
        }

        protected Task<bool?> ShowDialog(string dialogType, object dialogModel)
        {
            return this.dialogManager.ShowDialog(dialogType, dialogModel);
        }

        private Task HandleExceptionAsync(Exception x)
        {
            return this.dialogManager.ShowMessage(x.Message);
        }
    }
}