using System.Text.RegularExpressions;

namespace Infrastructure.Helpers
{
    internal static class Sanitizer
    {
        /// <summary>Remove everything illegal from project/category code</summary>
        /// <param name="code">Project/category code</param>
        /// <returns>Sanitized code</returns>
        internal static string SanitizeCode(string? code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            var rgx = new Regex("[^a-zA-Z0-9_-]", RegexOptions.Compiled | RegexOptions.CultureInvariant);
            var result = rgx.Replace(code, string.Empty).ToLower();

            if (string.IsNullOrEmpty(result))
            {
                throw new ArgumentNullException(nameof(code));
            }

            return result;
        }
    }
}
