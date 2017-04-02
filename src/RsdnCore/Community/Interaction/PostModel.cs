namespace Rsdn.Community.Interaction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PostModel : IIdentifiable, IVotes
    {
        public int Id { get; private set; }

        public int? ThreadId { get; private set; }

        public int? SubthreadId { get; private set; }

        public string Title { get; private set; }

        public string Message { get; private set; }

        public DateTime? Updated { get; private set; }

        public DateTime Posted { get; private set; }

        public string Username { get; private set; }

        public int? InterestingCount { get; private set; }
        public int? ThanksCount { get; private set; }
        public int? ExcellentCount { get; private set; }
        public int? AgreedCount { get; private set; }
        public int? DisagreedCount { get; private set; }
        public int? Plus1Count { get; private set; }
        public int? FunnyCount { get; private set; }

        public int Position { get; internal set; }

        public int Level { get; internal set; }

        public bool IsNew { get; internal set; }
    }
}