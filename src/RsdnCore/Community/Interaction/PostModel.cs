namespace Rsdn.Community.Interaction
{
    using System;

    public class PostModel : IIdentifiable, IVotes
    {
        public int Id { get; set; }

        public int? ThreadId { get; set; }

        public int? SubthreadId { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public DateTime? Updated { get; set; }

        public DateTime Posted { get; set; }

        public string Username { get; set; }

        public int? InterestingCount { get; set; }
        public int? ThanksCount { get; set; }
        public int? ExcellentCount { get; set; }
        public int? AgreedCount { get; set; }
        public int? DisagreedCount { get; set; }
        public int? Plus1Count { get; set; }
        public int? FunnyCount { get; set; }

        public int Position { get; internal set; }

        public int Level { get; internal set; }

        public bool IsNew { get; internal set; }
    }
}