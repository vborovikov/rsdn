namespace Rsdn.Community.Presentation.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
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

        public async Task ShowMessage(string content)
        {
            var messageDialog = new MessageDialog(content)
            {
                Commands =
                {
                    new UICommand("OK")
                }
            };
            await messageDialog.ShowAsync();
        }
    }
}