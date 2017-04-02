namespace Rsdn.Community.Interaction.Requests.Credentials
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Client;
    using Community;
    using Relay.RequestModel;

    public class CredentialControl :
        ICommandHandler<SigninCommand>,
        IQueryHandler<CredentialQuery, NetworkCredential>,
        IAsyncQueryHandler<VerifyCredentialQuery, CredentialVerificationResult>,
        IQueryHandler<HasCredentialQuery, bool>
    {
        private readonly ICredentialManager credentialMan;

        public CredentialControl(ICredentialManager credentialMan)
        {
            this.credentialMan = credentialMan;
        }

        public void Execute(SigninCommand command)
        {
            this.credentialMan.Credential = command.Credential;
        }

        public NetworkCredential Run(CredentialQuery query)
        {
            return this.credentialMan.Credential;
        }

        public bool Run(HasCredentialQuery query)
        {
            return this.credentialMan.HasCredential;
        }

        public Task<CredentialVerificationResult> RunAsync(VerifyCredentialQuery query)
        {
            return this.credentialMan.VerifyCredentialAsync(query.Credential, CancellationToken.None);
        }
    }
}