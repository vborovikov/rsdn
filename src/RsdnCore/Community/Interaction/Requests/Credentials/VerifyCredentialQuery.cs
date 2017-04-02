namespace Rsdn.Community.Interaction.Requests.Credentials
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Relay.RequestModel;

    public class VerifyCredentialQuery : QueryBase<CredentialVerificationResult>
    {
        public VerifyCredentialQuery(NetworkCredential credential)
        {
            this.Credential = credential;
        }

        public NetworkCredential Credential { get; }
    }
}