using System.Collections;
using System;

namespace GameBasic
{
    public class TimeUtil
    {
        public static long CurrentTimeSec
        {
            get { return (long)(DateTime.UtcNow - UTC_1970).TotalSeconds; }
        }

        public static long CurrentTimeMillis
        {
            get { return (long)(DateTime.UtcNow - UTC_1970).TotalMilliseconds; }
        }

        public static DateTime FromSec(long timeSpan)
        {
            return UTC_1970.AddSeconds(timeSpan);
        }

        public static DateTime FromMillis(long timeSpan)
        {
            return UTC_1970.AddMilliseconds(timeSpan);
        }

        public static DateTime FromSecLocal(long timeSpan)
        {
            return UTC_1970.AddSeconds(timeSpan).ToLocalTime();
        }

        public static DateTime FromMillisLocal(long timeSpan)
        {
            return FromMillis(timeSpan).ToLocalTime();
        }

        public static DateTime UTC_1970
        {
            get { return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); }
        }
    }
}