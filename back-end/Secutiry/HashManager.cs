using Abstractions.ISecurity;
using Abstractions.Model;
using Security.Helpers;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Security
{
    /// <summary> Hash manager </summary>
    public class HashManager : IHashManager
    {
        /// <summary> Hash plaint text</summary>
        /// <param name="plainText"> Plain text </param>
        /// <param name="hexSalt"> Salt as hex string. If hexSalt is null, the new salt will be created</param>
        /// <returns>Hash as hex string and hashed string</returns>
        public HashResult Hash(string plainText, string hexSalt = null)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentException("plainText must have value", nameof(plainText));

            if (hexSalt != null && string.IsNullOrEmpty(hexSalt))
                throw new ArgumentException("hexSalt must have value", nameof(hexSalt));


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


        /// <summary> Calculate hash of the plain text with salt</summary>
        /// <param name="plainText">Plain text </param>
        /// <param name="salt">Salt</param>
        /// <returns>Hash of the plain text with salt</returns>
        /// <exception cref="ArgumentNullException"/>
        private static byte[] CalcSaltedHash(byte[] plainText, byte[] salt)
        {
            var plainTextWithSaltBytes = new byte[plainText.Length + salt.Length];
            plainText.CopyTo(plainTextWithSaltBytes, 0);
            salt.CopyTo(plainTextWithSaltBytes, plainText.Length);
            
            using (var algorithm = new SHA384CryptoServiceProvider())
            {
                return algorithm.ComputeHash(plainTextWithSaltBytes);
            }
        }

        /// <summary>Create <see cref="byte"/> array filled with random numbers </summary>
        /// <param name="size"><see cref="byte"/> array size </param>
        /// <returns><see cref="byte"/> array filled with random numbers </returns>
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