using System;
using System.Globalization;

namespace MusicBeePlugin.Parsing
{
    public static class TimestampParser
    {
        public static bool TryParseTimestamp(string value, out TimeSpan timestamp)
        {
            timestamp = TimeSpan.Zero;

            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            string[] minuteAndSeconds = value.Split(':');
            if (minuteAndSeconds.Length != 2)
            {
                return false;
            }

            long minutes;
            if (!long.TryParse(minuteAndSeconds[0], NumberStyles.None, CultureInfo.InvariantCulture, out minutes) ||
                minutes < 0)
            {
                return false;
            }

            string secondsPart = minuteAndSeconds[1];
            int decimalIndex = secondsPart.IndexOfAny(new[] { '.', ',' });
            string secondsText = decimalIndex < 0 ? secondsPart : secondsPart.Substring(0, decimalIndex);
            int seconds;
            if (!int.TryParse(secondsText, NumberStyles.None, CultureInfo.InvariantCulture, out seconds) ||
                seconds < 0 ||
                seconds > 59)
            {
                return false;
            }

            int milliseconds = 0;
            if (decimalIndex >= 0)
            {
                string fraction = secondsPart.Substring(decimalIndex + 1);
                if (fraction.Length < 1 ||
                    fraction.Length > 3 ||
                    !int.TryParse(fraction, NumberStyles.None, CultureInfo.InvariantCulture, out milliseconds))
                {
                    return false;
                }

                if (fraction.Length == 1)
                {
                    milliseconds *= 100;
                }
                else if (fraction.Length == 2)
                {
                    milliseconds *= 10;
                }
            }

            try
            {
                long totalMilliseconds = checked((checked(minutes * 60) + seconds) * 1000 + milliseconds);
                timestamp = TimeSpan.FromTicks(checked(totalMilliseconds * TimeSpan.TicksPerMillisecond));
                return true;
            }
            catch (OverflowException)
            {
                return false;
            }
        }
    }
}