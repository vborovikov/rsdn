namespace Rsdn.Community
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IVotes
    {
        int? InterestingCount { get; }
        int? ThanksCount { get; }
        int? ExcellentCount { get; }
        int? AgreedCount { get; }
        int? DisagreedCount { get; }
        int? Plus1Count { get; }
        int? FunnyCount { get; }
    }
}