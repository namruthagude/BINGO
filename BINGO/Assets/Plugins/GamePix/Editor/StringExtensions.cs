using System.Text.RegularExpressions;

namespace GamePix.Editor
{
    public static class StringExtensions
    {
        private static readonly string pattern = @"[^a-zA-Z0-9_]";
        
        public static string ToSafeGpxString(this string value)
        {
            return Regex.Replace(value.Trim(), pattern, "_");
        }
    }
}