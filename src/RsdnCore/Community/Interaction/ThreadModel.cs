namespace Rsdn.Community.Interaction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ThreadModel : IIdentifiable, IVotes
    {
        public int Id { get; private set; }

        public string Title { get; private set; }

        public string Excerpt { get; private set; }

        public string Username { get; private set; }

        public DateTime Updated { get; private set; }

        public DateTime? Viewed { get; private set; }

        public int NewPostCount { get; private set; }

        public int PostCount { get; private set; }

        public int? InterestingCount { get; private set; }
        public int? ThanksCount { get; private set; }
        public int? ExcellentCount { get; private set; }
        public int? AgreedCount { get; private set; }
        public int? DisagreedCount { get; private set; }
        public int? Plus1Count { get; private set; }
        public int? FunnyCount { get; private set; }

        public bool IsNew { get; internal set; }
    }
}