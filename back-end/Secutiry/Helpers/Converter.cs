using System;
using System.IO;
using System.Text;

namespace Security.Helpers
{
    /// <summary> Converter </summary>
    internal static class Converter
    {
        /// <summary> Convert hex <see cref="string"/> to <see cref="byte[]"/> </summary>
        /// <param name="hexString">String to convert</param>
        /// <returns><see cref="byte[]"/></returns>
        internal static byte[] ToByteArray(string hexString)
        {
            if (string.IsNullOrEmpty(hexString))
                return null;

            var outputLength = hexString.Length / 2;
            var output = new byte[outputLength];
            using (var sr = new StringReader(hexString))
            {
                for (var i = 0; i < outputLength; i++)
                {
                    output[i] = Convert.ToByte(new string(new char[2] { (char)sr.Read(), (char)sr.Read() }), 16);
                }
            }
            return output;
        }

        /// <summary> Convert <see cref="byte[]"/> to hex <see cref="string"/> </summary>
        /// <param name="array">Array to convert</param>
        /// <returns>Hex <see cref="string"/></returns>
        internal static string ToHexString(byte[] array)
        {
            if (array == null || array.Length == 0)
                return null;

            var hex = new StringBuilder(array.Length * 2);
            foreach (var item in array)
            {
                hex.AppendFormat("{0:x2}", item);
            }

            return hex.ToString();
        }
    }
}