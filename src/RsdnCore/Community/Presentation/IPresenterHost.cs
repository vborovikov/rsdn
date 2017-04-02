namespace Rsdn.Community.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Relay.PresentationModel;
    using Relay.RequestModel;

    public interface IPresenterHost
    {
        Task ExecuteCommandAsync(ICommand command);

        Task<T> RunQueryAsync<T>(IQuery<T> query);

        void Navigate<TPresenter>(object parameter) where TPresenter : Presenter;

        Task ShowMessage(string content);

        Task<bool?> ShowDialog(string dialogType, object dialogModel);
    }
}