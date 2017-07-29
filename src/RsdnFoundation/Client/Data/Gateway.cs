namespace Rsdn.Client.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;
    using Polly;

    public abstract class Gateway : IGateway
    {
        protected static readonly Policy updatePolicy;

        static Gateway()
        {
            updatePolicy = Policy
                .Handle<DbException>()
                .WaitAndRetry(2, r => TimeSpan.FromSeconds(1));
        }
    }
}