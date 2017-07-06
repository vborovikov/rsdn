namespace Rsdn.Xaml.Controls
{
    using System.Windows.Input;
    using Microsoft.Toolkit.Uwp.UI.Controls;
    using Windows.UI.Xaml;

    public sealed class PullToRefreshMasterDetailsView : MasterDetailsView
    {
        public static readonly DependencyProperty RefreshCommandProperty =
            DependencyProperty.Register(nameof(RefreshCommand), typeof(ICommand),
                typeof(PullToRefreshMasterDetailsView), new PropertyMetadata(null));

        public PullToRefreshMasterDetailsView()
        {
            this.DefaultStyleKey = typeof(PullToRefreshMasterDetailsView);
        }

        public ICommand RefreshCommand
        {
            get { return (ICommand)GetValue(RefreshCommandProperty); }
            set { SetValue(RefreshCommandProperty, value); }
        }
    }
}