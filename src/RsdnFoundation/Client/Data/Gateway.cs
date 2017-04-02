namespace Rsdn.Client.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class Gateway : IGateway
    {
        protected const string RatingsJoin =
            "left outer join " +
            "(select Rating.PostId, " +
            "sum(case when Rating.Value = 1 then 1 else 0 end) as InterestingCount, " +
            "sum(case when Rating.Value = 2 then 1 else 0 end) as ThanksCount, " +
            "sum(case when Rating.Value = 3 then 1 else 0 end) as ExcellentCount, " +
            "sum(case when Rating.Value = -4 then 1 else 0 end) as AgreedCount, " +
            "sum(case when Rating.Value = 0 then 1 else 0 end) as DisagreedCount, " +
            "sum(case when Rating.Value = -3 then 1 else 0 end) as Plus1Count, " +
            "sum(case when Rating.Value = -2 then 1 else 0 end) as FunnyCount " +
            "from Rating " +
            "group by Rating.PostId " +
            ") as Ratings " +
            "on Post.Id = Ratings.PostId ";
    }
}