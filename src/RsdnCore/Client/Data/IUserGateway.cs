namespace Rsdn.Client.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Community.Interaction;

    public interface IUserGateway : IGateway
    {
        IEnumerable<ThreadModel> GetActivity(int userId);
    }
}