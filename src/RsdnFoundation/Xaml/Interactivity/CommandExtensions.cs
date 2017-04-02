namespace Rsdn.Xaml.Interactivity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public static class CommandExtensions
    {
        public static void TryExecute(this ICommand command, object parameter)
        {
            if (command.CanExecute(parameter))
                command.Execute(parameter);
        }
    }
}