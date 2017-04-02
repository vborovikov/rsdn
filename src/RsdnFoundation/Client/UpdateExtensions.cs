namespace Rsdn.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Janus;

    internal static class UpdateExtensions
    {
        private static readonly TimeZoneInfo rsdnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");

        public static int GetThreadId(this JanusMessageInfo msg) =>
            msg.topicId != 0 ? msg.topicId : msg.messageId;

        public static DateTime FromRsdnTimeToUtc(this DateTime dateTime) =>
            TimeZoneInfo.ConvertTime(dateTime, rsdnTimeZone, TimeZoneInfo.Utc);
    }
}