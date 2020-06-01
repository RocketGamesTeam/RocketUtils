using System;

namespace RocketUtils
{
    public static class DateTimeExtensions
    {
        private static DateTime _max = FromUnixTimestamp(int.MaxValue);

        public static DateTime Max
        {
            get { return _max; }
        }

        /// <summary>
        /// Converts a given DateTime into a Unix timestamp
        /// </summary>
        /// <param name="value">Any DateTime</param>
        /// <returns>The given DateTime in Unix timestamp format</returns>
        public static long ToUnixTimestamp(this DateTime value)
        {
            return (long)Math.Truncate((value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc))).TotalMilliseconds);
        }

        /// <summary>
        /// Gets a Unix timestamp representing the current moment
        /// </summary>
        /// <param name="ignored">Parameter ignored</param>
        /// <returns>Now expressed as a Unix timestamp</returns>
        public static long UnixTimestamp(this DateTime ignored)
        {
            return (long)Math.Truncate((DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds);
        }

        public static DateTime FromUnixTimestamp(int timestamp)
        {
            if (timestamp <= 0)
                return DateTime.MinValue;

            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(timestamp);

            return dateTime;
        }

        public static DateTime FromUnixTimestamp(long timestamp)
        {
            if (timestamp <= 0)
                return DateTime.MinValue;

            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddMilliseconds(timestamp);

            return dateTime;
        }
    }
}