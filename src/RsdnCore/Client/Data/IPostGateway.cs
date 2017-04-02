namespace Rsdn.Client.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Community.Interaction;

    public interface IPostGateway : IGateway
    {
        ThreadModel GetThread(int threadId);

        IEnumerable<PostModel> GetThreadPosts(int threadId);

        void MarkThreadAsViewed(int threadId);
    }
}