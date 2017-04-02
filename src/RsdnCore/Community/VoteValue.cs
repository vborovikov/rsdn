namespace Rsdn.Community
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public enum VoteValue
    {
        Interesting = 1,
        Thanks = 2,
        Excellent = 3,
        Agreed = -4,
        Disagreed = 0,
        Plus1 = -3,
        Funny = -2,
        None = -1,
    }
}