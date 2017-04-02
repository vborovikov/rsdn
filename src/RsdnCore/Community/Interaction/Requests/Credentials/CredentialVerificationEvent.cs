namespace Rsdn.Community.Interaction.Requests.Credentials
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Community;
    using Relay.RequestModel;

    public class CredentialVerificationEvent : EventBase
    {
        public CredentialVerificationEvent(CredentialVerificationResult result)
        {
            this.CredentialVerificationResult = result;
        }

        public CredentialVerificationResult CredentialVerificationResult { get; }
    }
}