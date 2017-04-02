namespace Rsdn.Xaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Community.Presentation;
    using Interactivity;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public sealed partial class Shell : Page
    {
        public Shell()
        {
            this.InitializeComponent();
        }

        private void HandleForumClick(object sender, RoutedEventArgs e)
        {
            //fixme: somehow binding in that button doesn't work
            (this.DataContext as ShellViewModel)?.ForumCommand.TryExecute((sender as Button)?.CommandParameter);
        }
    }
}