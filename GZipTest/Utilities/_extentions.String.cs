using System;
using System.Linq;

namespace GZipTest.Utilities
{
    internal static partial class _extentions
    {
        public static bool IsNullOrEmptyOrwhitespace(this string src) 
        {
            var res = src == null || src.Length == 0 || src.All(c => c == ' ' || c == '\t' || c == '\r' || c == '\n');
            return res;
        }

        public static string ToFormatString(this TimeSpan? interval)
        {
            var res = interval?.ToString("hh\\:mm\\:ss") ?? "-";
            return res;
        }
    }
}
