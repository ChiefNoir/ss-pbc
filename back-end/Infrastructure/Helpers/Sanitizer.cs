using System.Text.RegularExpressions;

namespace Infrastructure.Helpers
{
    internal static class Sanitizer
    {
        /// <summary>Remove everything illegal from project/category code</summary>
        /// <param name="code">Project/category code</param>
        /// <returns>Sanitized code</returns>
        internal static string SanitizeCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return null;

            var rgx = new Regex("[^a-zA-Z0-9_-]", RegexOptions.Compiled | RegexOptions.CultureInvariant);
            return rgx.Replace(code, "").ToLower();
        }
    }
}
