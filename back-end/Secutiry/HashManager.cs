using Abstractions.ISecurity;
using Abstractions.Model;
using Secutiry.Helpers;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Secutiry
{
    /// <summary> Hash manager </summary>
    public class HashManager : IHashManager
    {
        /// <summary> Calculate hash of the plain text with salt</summary>
        /// <param name="plainText">Plain text </param>
        /// <param name="salt">Salt</param>
        /// <returns>Hash of the plain text with salt</returns>
        /// <exception cref="ArgumentNullException"/>
        public static byte[] CalcSaltedHash(byte[] plainText, byte[] salt)
        {
            if (plainText == null || plainText.Length == 0)
                throw new ArgumentNullException("Can't calculate hash: input text is empty.");

            if (salt == null || salt.Length == 0)
                throw new ArgumentNullException("Can't calculate hash: input salt is empty.");

            using (var algorithm = new SHA384CryptoServiceProvider())
            {
                var plainTextWithSaltBytes = new byte[plainText.Length + salt.Length];

                for (int i = 0; i < plainText.Length; i++)
                {
                    plainTextWithSaltBytes[i] = plainText[i];
                }
                for (int i = 0; i < salt.Length; i++)
                {
                    plainTextWithSaltBytes[plainText.Length + i] = salt[i];
                }

                return algorithm.ComputeHash(plainTextWithSaltBytes);
            }
        }

        /// <summary> Hash plaint text</summary>
        /// <param name="plainText"> Plain text </param>
        /// <param name="hexSalt"> Salt as hex string. If hexSalt is null, the new salt will be created</param>
        /// <returns>Hash as hex string and hashed string</returns>
        public HashResult Hash(string plainText, string hexSalt = null)
        {
            byte[] salt;
            if (string.IsNullOrEmpty(hexSalt))
                salt = GenerateRandomArray(128);
            else
                salt = Converter.ToByteArray(hexSalt);

            var result = CalcSaltedHash(Encoding.UTF8.GetBytes(plainText), salt);

            return new HashResult
            {
                HexHash = Converter.ToHexString(result),
                HexSalt = hexSalt ?? Converter.ToHexString(salt)
            };
        }

        /// <summary>Create <see cref="byte[]"/> filled with random numbers </summary>
        /// <param name="size"><see cref="byte[]"/> size </param>
        /// <returns><see cref="byte[]"/> filled with random numbers </returns>
        private static byte[] GenerateRandomArray(int size)
        {
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                var result = new byte[size];
                rngCsp.GetNonZeroBytes(result);

                return result;
            }
        }
    }
}