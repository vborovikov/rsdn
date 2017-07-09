namespace Rsdn.Community.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IDialogManager
    {
        Task ShowMessage(string content);

        Task<bool?> ShowDialog(string dialogType, object dialogModel);

        Task RunAsync(Action action);
    }
}