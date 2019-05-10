using System;

namespace WebFormsMvc.Extensions
{
    public static class StringExtensions
    {
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