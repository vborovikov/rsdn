namespace Rsdn.Community.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Relay.PresentationModel;

    public abstract class DialogPresenter : Presenter, IDialogContext
    {
        private EventHandler closeRequested;

        event EventHandler IDialogContext.CloseRequested
        {
            add
            {
                this.closeRequested += value;
            }

            remove
            {
                this.closeRequested -= value;
            }
        }

        protected void CloseDialog()
        {
            OnCloseRequested(EventArgs.Empty);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
        }

        private void OnCloseRequested(EventArgs args)
        {
            var handler = this.closeRequested;
            if (handler != null)
                handler(this, args);
        }
    }
}