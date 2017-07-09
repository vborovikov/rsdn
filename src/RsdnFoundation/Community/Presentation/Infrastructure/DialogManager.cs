namespace Rsdn.Community.Presentation.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using Windows.ApplicationModel.Core;
    using Windows.UI.Core;
    using Windows.UI.Popups;
    using Windows.UI.Xaml.Controls;

    public class DialogManager : IDialogManager
    {
        public async Task<bool?> ShowDialog(string dialogType, object dialogModel)
        {
            var dialog = Activator.CreateInstance(Type.GetType(dialogType)) as ContentDialog;
            dialog.DataContext = dialogModel;
            var result = await dialog.ShowAsync();

            return
                result == ContentDialogResult.Primary ? true :
                result == ContentDialogResult.Secondary ? false :
                (bool?)null;
        }

        public Task ShowMessage(string content)
        {
            var messageDialog = new MessageDialog(content)
            {
                Commands =
                {
                    new UICommand("OK")
                }
            };
            return messageDialog.ShowAsync().AsTask();
        }

        public Task RunAsync(Action action)
        {
            return CoreApplication.MainView.CoreWindow.Dispatcher
                 .RunAsync(CoreDispatcherPriority.Low, () => action()).AsTask();
        }
    }
}