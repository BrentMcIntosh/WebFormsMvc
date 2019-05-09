using System;

namespace WebFormsMvc.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsExt(this string str, string val, StringComparison comparison)
        {
            return str.IndexOf(val, comparison) > -1;
        }

        public static string FirstCharToUpper(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}