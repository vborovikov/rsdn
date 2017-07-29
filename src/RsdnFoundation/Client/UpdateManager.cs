#define PRINT_UPDATE

namespace Rsdn.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Data.Fetch;
    using Data.Storage;
    using Janus;

    public class UpdateManager : IUpdateManager
    {
        private static readonly TimeSpan ForumExpirationPeriod = TimeSpan.FromDays(7);

        private readonly Fetcher fetcher;
        private readonly Keeper keeper;

        public UpdateManager(ICredentialManager credentialManager)
        {
            this.fetcher = new Fetcher(credentialManager.Credential);
            this.keeper = new Keeper();

            credentialManager.CredentialChanged += HandleCredentialChanged;
        }

        public async Task UpdateDirectoryAsync(CancellationToken cancelToken)
        {
            var directory = await this.fetcher.FetchDirectoryAsync(cancelToken);
            this.keeper.StoreDirectory(directory, cancelToken);
        }

        public async Task UpdateForumsAsync(IEnumerable<int> forumIds, CancellationToken cancelToken)
        {
            var forums = this.keeper.GetForums(forumIds);
            var forumRequests = forums
                .Select(f => new RequestForumInfo { forumId = f.Id, isFirstRequest = ForumIsExpired(f) })
                .ToArray();

            var changes = await this.fetcher.FetchChangesAsync(forumRequests, cancelToken);
            var orphans = this.keeper.StoreChanges(changes, cancelToken);

            IEnumerable<int> missingThreadIds = orphans
                .Select(p => p.GetThreadId())
                .Distinct().ToArray();

            // this must execute at least once in order to store stats
            do
            {
#if PRINT_UPDATE
                System.Diagnostics.Debug.WriteLine("Fetching old threads [{0}]", (object)String.Join(", ", missingThreadIds));
#endif
                var archiveThreadIds = Enumerable.Empty<int>();
                if (missingThreadIds.Any())
                {
                    var archives = await this.fetcher.FetchArchivesAsync(missingThreadIds, cancelToken);
                    this.keeper.StoreArchives(archives, cancelToken);

                    archiveThreadIds = archives.Messages.Select(p => p.GetThreadId());
                }

                var threadIds = changes.newMessages.Select(p => p.GetThreadId())
                    .Concat(archiveThreadIds)
                    .Distinct().ToArray();
                missingThreadIds = this.keeper.StoreStats(threadIds, cancelToken);
            }
            while (missingThreadIds.Any());
        }

        private static bool ForumIsExpired(DbForum forum)
        {
            if (forum.Fetched == null || (DateTime.UtcNow - forum.Fetched) > ForumExpirationPeriod)
            {
                return true;
            }

            if (forum.Posted == null)
            {
                return true;
            }

            var postPeriod = DateTime.UtcNow - forum.Posted.Value;
            return postPeriod > ForumExpirationPeriod;
        }

        private void HandleCredentialChanged(object sender, EventArgs e)
        {
            var credentialManager = (ICredentialManager)sender;
            this.fetcher.Credential = credentialManager.Credential;
        }
    }
}