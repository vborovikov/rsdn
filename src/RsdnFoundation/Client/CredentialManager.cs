namespace Rsdn.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Threading;
    using System.Threading.Tasks;
    using Community;
    using Data.Fetch;
    using Janus;
    using Windows.Security.Credentials;

    public class CredentialManager : ICredentialManager
    {
        private const string ResourceName = "RSDN";

        public bool HasCredential
        {
            get
            {
                try
                {
                    var vault = new PasswordVault();
                    var allCredentials = vault.FindAllByResource(ResourceName);
                    if (allCredentials != null)
                        return allCredentials.Any();
                }
                catch (Exception)
                {
                }

                return false;
            }
        }

        public NetworkCredential Credential
        {
            get { return GetCredential(); }
            set { SetCredential(value); }
        }

        public event EventHandler CredentialChanged;

        public async Task<CredentialVerificationResult> VerifyCredentialAsync(NetworkCredential credential, CancellationToken cancelToken)
        {
            if (credential.IsEmpty())
                return CredentialVerificationResult.NoCredential;
            if (credential.IsInvalid())
                return CredentialVerificationResult.SpellingError;

            // check network

            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                return CredentialVerificationResult.NetworkError;
            }

            // check web-service

            var webServiceIsUpAndRunning = await ServiceWrapper.CheckAsync();
            if (webServiceIsUpAndRunning == false)
            {
                return CredentialVerificationResult.WebServiceDown;
            }

            using (var wrapper = new ServiceWrapper(cancelToken))
            {
                var service = wrapper.Service;
                // check web-service call

                try
                {
                    var users = await service.GetUserByIdsAsync(new UserByIdsRequest
                    {
                        userName = credential.UserName,
                        password = credential.Password,
                        userIds = new ArrayOfInt { 1 },
                    });

                    if (users.Body.GetUserByIdsResult.users.Any() == false)
                    {
                        return CredentialVerificationResult.WrongCredential;
                    }
                }
                catch (Exception)
                {
                    return CredentialVerificationResult.WrongCredential;
                }
            }

            return CredentialVerificationResult.Success;
        }

        protected virtual void OnCredentialChanged(EventArgs e)
        {
            this.CredentialChanged?.Invoke(this, e);
        }

        private NetworkCredential GetCredential()
        {
            try
            {
                var vault = new PasswordVault();
                var allCredentials = vault.FindAllByResource(ResourceName);
                if (allCredentials != null)
                {
                    var credential = allCredentials.FirstOrDefault();
                    if (credential != null)
                    {
                        credential.RetrievePassword();
                        return new NetworkCredential(credential.UserName, credential.Password);
                    }
                }
            }
            catch (Exception)
            {
            }

            return new NetworkCredential();
        }

        private void SetCredential(NetworkCredential record)
        {
            var vault = new PasswordVault();
            try
            {
                var allCredentials = vault.FindAllByResource(ResourceName);
                if (allCredentials != null)
                {
                    foreach (var credential in allCredentials)
                        vault.Remove(credential);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                var credential = new PasswordCredential(ResourceName, record.UserName, record.Password);
                vault.Add(credential);
                OnCredentialChanged(EventArgs.Empty);
            }
        }
    }
}