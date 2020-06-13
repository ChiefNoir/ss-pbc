using Abstractions.Model;

namespace Abstractions.ISecurity
{
    /// <summary> Hash manager </summary>
    public interface IHashManager
    {
        /// <summary> Hash plaint text</summary>
        /// <param name="plainText"> Plain text </param>
        /// <param name="hexSalt"> Salt as hex string. If hexSalt is null, the new salt will be created</param>
        /// <returns>Hash as hex string and hashed string</returns>
        HashResult Hash(string plainText, string hexSalt = null);
    }
}
