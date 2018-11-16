using System;
using System.Text.RegularExpressions;

namespace SD.Services.Data.Utils
{
    internal static class Validator
    {
        public static void ValidateLengthRange(int value, int min, int max, string message)
        {
            if (value < min || value > max)
            {
                throw new ArgumentOutOfRangeException(message);
            }
        }

        public static void ValidateNull(Object value, string message)
        {
            if (value == null)
            {
                throw new ArgumentNullException(message);
            }
        }

        public static void ValidateMinRange(int value, int min, string message)
        {
            if (value < min)
            {
                throw new ArgumentOutOfRangeException(message);
            }
        }

        public static void ValidateSymbol(string value, string pattern, string message)
        {
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            if (!regex.IsMatch(value))
            {
                throw new ArgumentException(message);
            }
        }

        public static void ValidateGuid(string value, string message)
        {
            if (!Guid.TryParse(value, out Guid temp))
            {
                throw new ArgumentException(message);
            }
        }
    }
}
