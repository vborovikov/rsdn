namespace Rsdn.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IUpdateManager
    {
        Task UpdateDirectoryAsync(CancellationToken cancelToken);

        Task UpdateForumsAsync(IEnumerable<int> forumIds, CancellationToken cancelToken);
    }
}