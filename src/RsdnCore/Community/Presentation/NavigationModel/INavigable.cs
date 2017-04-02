namespace Rsdn.Community.Presentation.NavigationModel
{
    using System.Threading.Tasks;

    public interface INavigable
    {
        Task OnNavigatedToAsync(object parameter);

        Task OnNavigatedFromAsync(object parameter);
    }
}