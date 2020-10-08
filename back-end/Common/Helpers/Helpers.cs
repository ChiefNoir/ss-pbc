using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Common.Helpers
{
    public static class Helpers
    {
        // https://msdn.microsoft.com/en-us/library/aa365247.aspx#naming_conventions
        // http://stackoverflow.com/questions/146134/how-to-remove-illegal-characters-from-path-and-filenames
        private static readonly Regex removeInvalidChars = 
            new Regex
            (
                $"[{Regex.Escape(new string(Path.GetInvalidFileNameChars()))}]",
                RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.CultureInvariant
            );

        public static string SanitizedFileName(string fileName, string replacement = "_")
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            return removeInvalidChars.Replace(fileName, replacement);
        }

    }
}
