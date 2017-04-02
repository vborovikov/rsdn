namespace Rsdn.Community.Interaction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ForumStatus
    {
        public ForumStatus(int id, bool favorite,
            DateTime? fetched, DateTime? visited,
            DateTime? posted, int? postCount)
        {
            this.Id = id;
            this.IsFavorite = favorite;

            var oneDayBefore = DateTime.UtcNow.AddDays(-1d);
            var weekBefore = DateTime.UtcNow.AddDays(-7d);
            var oneHour = TimeSpan.FromHours(1);

            this.IsRecentlyVisited = visited > oneDayBefore;
            this.IsOutdated = fetched == null || fetched < weekBefore;
            this.IsPopular = (fetched - posted) < oneHour && postCount > 1000;
            this.IsAbandoned = fetched != null && (posted == null || posted < weekBefore || postCount < 100);
        }

        public int Id { get; }
        public bool IsFavorite { get; }
        public bool IsRecentlyVisited { get; }
        public bool IsOutdated { get; }
        public bool IsPopular { get; }
        public bool IsAbandoned { get; }
    }
}