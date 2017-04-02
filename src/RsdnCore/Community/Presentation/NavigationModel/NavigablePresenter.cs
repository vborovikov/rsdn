namespace Rsdn.Community.Presentation.NavigationModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Relay.PresentationModel;

    public abstract class NavigablePresenter : Presenter, INavigable, ITombstone
    {
        protected NavigablePresenter()
        {
        }

        Task INavigable.OnNavigatedFromAsync(object parameter)
        {
            return this.OnNavigatedFromAsync(parameter);
        }

        Task INavigable.OnNavigatedToAsync(object parameter)
        {
            return this.OnNavigatedToAsync(parameter);
        }

        Task ITombstone.OnSerializingAsync(IDictionary<string, object> state)
        {
            return this.OnSerializingAsync(state);
        }

        Task ITombstone.OnDeserializingAsync(IDictionary<string, object> state)
        {
            return this.OnDeserializingAsync(state);
        }

        protected virtual Task OnNavigatedFromAsync(object parameter)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnNavigatedToAsync(object parameter)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnSerializingAsync(IDictionary<string, object> state)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnDeserializingAsync(IDictionary<string, object> state)
        {
            return Task.CompletedTask;
        }
    }
}