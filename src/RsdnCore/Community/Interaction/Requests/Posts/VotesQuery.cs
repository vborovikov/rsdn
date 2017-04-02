namespace Rsdn.Community.Interaction.Requests.Posts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Relay.RequestModel;

    public class VotesQuery : QueryBase<IEnumerable<ThreadDetails>>
    {
    }
}