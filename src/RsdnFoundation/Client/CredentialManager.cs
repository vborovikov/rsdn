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
    using Windows.Data.Json;
    using Windows.Security.Credentials;
    using Windows.Storage;

    public class CredentialManager : ICredentialManager
    {
        private const string ResourceName = "RSDN";

        private static readonly ApplicationDataContainer localSettings;

        static CredentialManager()
        {
            localSettings = ApplicationData.Current.LocalSettings;
        }

        public CredentialManager()
        {
            if (RestoreUser() == false)
            {
                this.User = new UserModel
                {
                    Id = -1,
                    Alias = String.Empty,
                };
            }
        }

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

        public UserModel User { get; private set; }

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
                    var posts = await service.GetNewDataAsync(new ChangeRequest
                    {
                        userName = credential.UserName,
                        password = credential.Password,
                        maxOutput = 0,
                        messageRowVersion = new byte[8],
                        ratingRowVersion = new byte[8],
                        moderateRowVersion = new byte[8],
                        subscribedForums = new RequestForumInfo[]
                        {
                            new RequestForumInfo
                            {
                                forumId = 1,
                                isFirstRequest = true
                            }
                        }
                    });

                    if (posts.Body.GetNewDataResult.userId == 0)
                    {
                        return CredentialVerificationResult.WrongCredential;
                    }
                    else
                    {
                        var userId = posts.Body.GetNewDataResult.userId;

                        var users = await service.GetUserByIdsAsync(new UserByIdsRequest
                        {
                            userName = credential.UserName,
                            password = credential.Password,
                            userIds = new ArrayOfInt { userId },
                        });

                        var user = users.Body.GetUserByIdsResult.users.FirstOrDefault();
                        if (user == null)
                        {
                            return CredentialVerificationResult.WrongCredential;
                        }

                        this.User = new UserModel
                        {
                            Id = user.userId,
                            Alias = user.userNick,
                            Name = user.userName,
                            RealName = user.realName,
                            Origin = user.whereFrom,
                            Address = user.homePage,
                            Email = user.publicEmail,
                            Interests = user.specialization,
                            Signature = user.origin,
                        };
                        StoreUser();
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

        private bool RestoreUser()
        {
            var userStr = localSettings.Values[nameof(this.User)] as string;
            if (String.IsNullOrWhiteSpace(userStr) == false)
            {
                JsonObject userJson;
                if (JsonObject.TryParse(userStr, out userJson))
                {
                    this.User = new UserModel
                    {
                        Id = Convert.ToInt32(userJson.GetNamedNumber(nameof(UserModel.Id))),
                        Alias = userJson.GetNamedString(nameof(UserModel.Alias), String.Empty),
                        Name = userJson.GetNamedString(nameof(UserModel.Name), String.Empty),
                        RealName = userJson.GetNamedString(nameof(UserModel.RealName), String.Empty),
                        Origin = userJson.GetNamedString(nameof(UserModel.Origin), String.Empty),
                        Address = userJson.GetNamedString(nameof(UserModel.Address), String.Empty),
                        Email = userJson.GetNamedString(nameof(UserModel.Email), String.Empty),
                        Interests = userJson.GetNamedString(nameof(UserModel.Interests), String.Empty),
                        Signature = userJson.GetNamedString(nameof(UserModel.Signature), String.Empty),
                    };

                    return true;
                }
            }

            return false;
        }

        private void StoreUser()
        {
            var userJson = new JsonObject();

            userJson[nameof(UserModel.Id)] = JsonValue.CreateNumberValue(this.User.Id);
            userJson[nameof(UserModel.Alias)] = JsonValue.CreateStringValue(this.User.Alias);
            userJson[nameof(UserModel.Name)] = JsonValue.CreateStringValue(this.User.Name);
            userJson[nameof(UserModel.RealName)] = JsonValue.CreateStringValue(this.User.RealName);
            userJson[nameof(UserModel.Origin)] = JsonValue.CreateStringValue(this.User.Origin);
            userJson[nameof(UserModel.Address)] = JsonValue.CreateStringValue(this.User.Address);
            userJson[nameof(UserModel.Email)] = JsonValue.CreateStringValue(this.User.Email);
            userJson[nameof(UserModel.Interests)] = JsonValue.CreateStringValue(this.User.Interests);
            userJson[nameof(UserModel.Signature)] = JsonValue.CreateStringValue(this.User.Signature);

            localSettings.Values[nameof(this.User)] = userJson.Stringify();
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