namespace Rsdn.Community.Interaction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ThreadModel : IIdentifiable, IVotes
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Excerpt { get; set; }

        public string Username { get; set; }

        public DateTime Updated { get; set; }

        public DateTime? Viewed { get; set; }

        public int NewPostCount { get; set; }

        public int PostCount { get; set; }

        public int? InterestingCount { get; set; }
        public int? ThanksCount { get; set; }
        public int? ExcellentCount { get; set; }
        public int? AgreedCount { get; set; }
        public int? DisagreedCount { get; set; }
        public int? Plus1Count { get; set; }
        public int? FunnyCount { get; set; }

        public bool IsNew { get; set; }
    }
}