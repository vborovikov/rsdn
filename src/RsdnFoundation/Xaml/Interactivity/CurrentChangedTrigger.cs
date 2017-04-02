namespace Rsdn.Xaml.Interactivity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Microsoft.Xaml.Interactivity;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    public class CurrentChangedTrigger : Behavior<CollectionViewSource>
    {
        public static readonly DependencyProperty CallbackProperty =
            DependencyProperty.Register(nameof(Callback), typeof(ICommand),
                typeof(CurrentChangedTrigger), new PropertyMetadata(null));

        private long regToken;

        public ICommand Callback
        {
            get { return (ICommand)GetValue(CallbackProperty); }
            set { SetValue(CallbackProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (this.AssociatedObject.View == null)
            {
                this.regToken = this.AssociatedObject.RegisterPropertyChangedCallback(CollectionViewSource.ViewProperty, HandleCollectionViewChanged);
            }
            else
            {
                this.AssociatedObject.View.CurrentChanged += HandleCollectionViewCurrentItemChanged;
            }
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.View.CurrentChanged -= HandleCollectionViewCurrentItemChanged;
            base.OnDetaching();
        }

        private void HandleCollectionViewChanged(DependencyObject sender, DependencyProperty dp)
        {
            if (this.AssociatedObject.View != null)
            {
                this.AssociatedObject.UnregisterPropertyChangedCallback(CollectionViewSource.ViewProperty, this.regToken);
                this.AssociatedObject.View.CurrentChanged += HandleCollectionViewCurrentItemChanged;
            }
        }

        private void HandleCollectionViewCurrentItemChanged(object sender, object e)
        {
            this.Callback?.TryExecute(this.AssociatedObject.View.CurrentItem);
        }
    }
}