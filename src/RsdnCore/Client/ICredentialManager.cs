namespace Rsdn.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Community;

    public interface ICredentialManager
    {
        bool HasCredential { get; }

        NetworkCredential Credential { get; set; }

        event EventHandler CredentialChanged;

        Task<CredentialVerificationResult> VerifyCredentialAsync(NetworkCredential credential, CancellationToken cancelToken);
    }
}