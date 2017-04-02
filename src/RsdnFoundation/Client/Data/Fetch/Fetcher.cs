namespace Rsdn.Client.Data.Fetch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Janus;
    using Windows.Storage;

    internal class Fetcher
    {
        private const int RowVersionSize = 8;

        private static readonly ApplicationDataContainer roamingSettings;
        private static byte[] forumsRowVersion;
        private static byte[] messageRowVersion;
        private static byte[] ratingRowVersion;
        private static byte[] moderateRowVersion;

        static Fetcher()
        {
            roamingSettings = ApplicationData.Current.RoamingSettings;
            ApplicationData.Current.DataChanged += HandleRoamingDataChanged;

            RestoreRowVersions();

            if (forumsRowVersion == null)
                forumsRowVersion = new byte[RowVersionSize];
            if (messageRowVersion == null)
                messageRowVersion = new byte[RowVersionSize];
            if (ratingRowVersion == null)
                ratingRowVersion = new byte[RowVersionSize];
            if (moderateRowVersion == null)
                moderateRowVersion = new byte[RowVersionSize];
        }

        public Fetcher(NetworkCredential credential)
        {
            this.Credential = credential;
        }

        public NetworkCredential Credential { get; set; }

        public async Task<ForumResponse> FetchDirectoryAsync(CancellationToken cancelToken)
        {
            using (var wrapper = new ServiceWrapper(cancelToken))
            {
                var service = wrapper.Service;
                var result = await service.GetForumListAsync(new ForumRequest
                {
                    userName = this.Credential.UserName,
                    password = this.Credential.Password,
                    forumsRowVersion = forumsRowVersion,
                });

                Interlocked.Exchange(ref forumsRowVersion, result.Body.GetForumListResult.forumsRowVersion);
                StoreRowVersions();

                return result.Body.GetForumListResult;
            }
        }

        public async Task<ChangeResponse> FetchChangesAsync(IEnumerable<RequestForumInfo> forums, CancellationToken cancelToken)
        {
            using (var wrapper = new ServiceWrapper(cancelToken))
            {
                var service = wrapper.Service;
                var result = await service.GetNewDataAsync(new ChangeRequest
                {
                    userName = this.Credential.UserName,
                    password = this.Credential.Password,
                    subscribedForums = forums.ToArray(),
                    maxOutput = -1,
                    messageRowVersion = messageRowVersion,
                    ratingRowVersion = ratingRowVersion,
                    moderateRowVersion = moderateRowVersion,
                });

                Interlocked.Exchange(ref messageRowVersion, result.Body.GetNewDataResult.lastForumRowVersion);
                Interlocked.Exchange(ref ratingRowVersion, result.Body.GetNewDataResult.lastRatingRowVersion);
                Interlocked.Exchange(ref moderateRowVersion, result.Body.GetNewDataResult.lastModerateRowVersion);
                StoreRowVersions();

                return result.Body.GetNewDataResult;
            }
        }

        public async Task<TopicResponse> FetchArchivesAsync(IEnumerable<int> threadIds, CancellationToken cancelToken)
        {
            if (threadIds.Any() == false)
                return new TopicResponse { Messages = new JanusMessageInfo[0] };

            using (var wrapper = new ServiceWrapper(cancelToken))
            {
                var service = wrapper.Service;
                var result = await service.GetTopicByMessageAsync(new TopicRequest
                {
                    userName = this.Credential.UserName,
                    password = this.Credential.Password,
                    messageIds = CreateArrayOfInt(threadIds)
                });

                return result.Body.GetTopicByMessageResult;
            }
        }

        private static void HandleRoamingDataChanged(ApplicationData sender, object args)
        {
            RestoreRowVersions();
        }

        private static void RestoreRowVersions()
        {
            Interlocked.Exchange(ref forumsRowVersion, roamingSettings.Values[nameof(forumsRowVersion)] as byte[]);
            Interlocked.Exchange(ref messageRowVersion, roamingSettings.Values[nameof(messageRowVersion)] as byte[]);
            Interlocked.Exchange(ref ratingRowVersion, roamingSettings.Values[nameof(ratingRowVersion)] as byte[]);
            Interlocked.Exchange(ref moderateRowVersion, roamingSettings.Values[nameof(moderateRowVersion)] as byte[]);
        }

        private static void StoreRowVersions()
        {
            roamingSettings.Values[nameof(forumsRowVersion)] = forumsRowVersion;
            roamingSettings.Values[nameof(messageRowVersion)] = messageRowVersion;
            roamingSettings.Values[nameof(ratingRowVersion)] = ratingRowVersion;
            roamingSettings.Values[nameof(moderateRowVersion)] = moderateRowVersion;
        }

        private static ArrayOfInt CreateArrayOfInt(IEnumerable<int> integers)
        {
            var arrayOfInt = new ArrayOfInt();
            arrayOfInt.AddRange(integers);
            return arrayOfInt;
        }
    }
}