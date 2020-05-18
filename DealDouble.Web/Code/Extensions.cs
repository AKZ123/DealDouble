using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace DealDouble.Web.Code
{
    public static class Extensions
    {
        private static string illegalCharacterReplacePattern = @"[^\w]";

        public static string SanitizeString(this string str)
        {
            string sanitizedString = string.Empty;
            if (!string.IsNullOrEmpty(str))
            {
                sanitizedString = Regex.Replace(str.Trim(), illegalCharacterReplacePattern, "-");
                sanitizedString = sanitizedString.Replace("---", "-").Replace("--", "-");
                sanitizedString = sanitizedString.TrimStart('-').TrimEnd('-');
            }
            return sanitizedString;
        }
    }
}