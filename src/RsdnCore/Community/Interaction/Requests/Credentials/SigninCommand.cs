namespace Rsdn.Community.Interaction.Requests.Credentials
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Relay.RequestModel;

    public class SigninCommand : CommandBase
    {
        public SigninCommand(string username, string password)
            : this(new NetworkCredential(username, password))
        {
        }

        public SigninCommand(NetworkCredential credential)
        {
            this.Credential = credential;
        }

        public NetworkCredential Credential { get; }
    }
}