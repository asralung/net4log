namespace LogfileReader
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>The log file parser extensions.</summary>
    internal static class LogFileParserExtensions
    {
        /// <summary>The date time format.</summary>
        internal const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss,fff";

        /// <summary>The length of time stamp.</summary>
        private static readonly int LengthOfTimeStamp = DateTimeFormat.Length;

        /// <summary>The is start of new record.</summary>
        /// <param name="line">The line.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsStartOfNewRecord(this string line)
        {
            if (line.Length < LengthOfTimeStamp)
            {
                return false;
            }

            var substring = line.Substring(0, LengthOfTimeStamp);

            DateTime time;
            return DateTime.TryParseExact(substring, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,  out time);
        }

        /// <summary>The get record time stamp.</summary>
        /// <param name="first">The first.</param>
        /// <returns>The <see cref="DateTime"/>.</returns>
        public static DateTime GetRecordTimeStamp(this string first)
        {
            var substring = first.Substring(0, LengthOfTimeStamp);
            var timeStamp = DateTime.ParseExact(substring, DateTimeFormat, CultureInfo.InvariantCulture);
            return timeStamp;
        }

        /// <summary>Gets the transaction id from a given line when available.</summary>
        /// <param name="line">The line.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetTransActionId(this string line)
        {
            var parts = line.Split(';');
            var part = parts.FirstOrDefault(x => x.Contains("Transaction ID"));
            if (part != default(string))
            {
                var parts2 = part.Split('=');
                if (parts2.Length > 1)
                {
                    return parts2[1].Trim();
                }
            }

            return string.Empty;
        }

        /// <summary>The contains SX FY.</summary>
        /// <param name="lines">The lines.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool ContainsSxFy(this IList<string> lines)
        {
            return lines.Any(line => line.Contains("Transaction ID"));
        }
    }
}