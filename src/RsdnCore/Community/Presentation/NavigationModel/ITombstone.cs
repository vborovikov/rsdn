namespace Rsdn.Community.Presentation.NavigationModel
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITombstone
    {
        Task OnSerializingAsync(IDictionary<string, object> state);

        Task OnDeserializingAsync(IDictionary<string, object> state);
    }
}