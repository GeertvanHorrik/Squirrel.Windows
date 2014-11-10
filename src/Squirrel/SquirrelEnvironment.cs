using System;
using System.Globalization;

namespace Squirrel
{
    public static class SquirrelEnvironment
    {
        public const string DateTimePattern = "yyyyMMddHHmmss";

        public static DateTime ParseDateTime(string s, DateTime defaultDateTime)
        {
            var dateTime = defaultDateTime;

            if (!DateTime.TryParseExact(s, DateTimePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                dateTime = defaultDateTime;
            }

            return dateTime;
        }
    }
}
