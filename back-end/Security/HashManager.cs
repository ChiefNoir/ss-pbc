using Microsoft.Extensions.Configuration;
using Security.Helpers;
using Security.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Security
{
    /// <summary> Hash manager </summary>
    public class HashManager
    {
        private readonly IConfiguration _configuration;

        public HashManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary> Hash plaint text</summary>
        /// <param name="plainText"> Plain text </param>
        /// <param name="hexSalt"> Salt as hex string. If hexSalt is null, the new salt will be created</param>
        /// <returns>Hash as hex string and hashed string</returns>
        public HashResult Hash(string plainText, string hexSalt = null)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                throw new ArgumentException("plainText must have value", nameof(plainText));
            }

            if (hexSalt != null && string.IsNullOrEmpty(hexSalt))
            {
                throw new ArgumentException("hexSalt must have value", nameof(hexSalt));
            }

            var text = AddExtraSalt
            (
                Encoding.UTF8.GetBytes(plainText),
                Encoding.UTF8.GetBytes(_configuration.GetSection("Security:Key").Get<string>())
            );

            byte[] salt;
            if (string.IsNullOrEmpty(hexSalt))
            {
                salt = GenerateRandomArray(128);
            }
            else
            {
                salt = HexConverter.ToByteArray(hexSalt);
            }

            var result = CalcSaltedHash(text, salt);
            return new HashResult
            {
                HexHash = HexConverter.ToHexString(result),
                HexSalt = hexSalt ?? HexConverter.ToHexString(salt)
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

            using (var algorithm = SHA384.Create())
            {
                return algorithm.ComputeHash(plainTextWithSaltBytes);
            }
        }

        /// <summary>Create <see cref="byte"/> array filled with random numbers </summary>
        /// <param name="size"><see cref="byte"/> array size </param>
        /// <returns><see cref="byte"/> array filled with random numbers </returns>
        private static byte[] GenerateRandomArray(int size)
        {
            using (var rngCsp = RandomNumberGenerator.Create())
            {
                var result = new byte[size];
                rngCsp.GetNonZeroBytes(result);

                return result;
            }
        }

        /// <summary> Add extra salt to the plain text</summary>
        /// <param name="plainText">Plain text</param>
        /// <param name="salt">Salt</param>
        /// <returns>Plain text with extra salt</returns>
        private static byte[] AddExtraSalt(byte[] plainText, byte[] salt)
        {
            var plainTextWithSaltBytes = new byte[plainText.Length + salt.Length];
            plainText.CopyTo(plainTextWithSaltBytes, 0);
            salt.CopyTo(plainTextWithSaltBytes, plainText.Length);

            return plainTextWithSaltBytes;
        }
    }
}