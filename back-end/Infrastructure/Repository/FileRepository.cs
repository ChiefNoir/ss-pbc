using Abstractions.IRepository;
using Common.FriendlyConverters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.IO;

namespace Infrastructure.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly IConfiguration _configuration;
        private readonly int _maxFileSize;
        private readonly string _fileStorage;


        public FileRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _maxFileSize = _configuration.GetSection("Location:MaxFileSizeBytes").Get<int>();
            _fileStorage = _configuration.GetSection("Location:FileStorage").Get<string>();
        }

        public string Save(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException(Resources.TextMessages.FileIsEmpty);
            }

            if (file.Length > _maxFileSize)
            {
                throw new ArgumentException
                   (
                        string.Format(Resources.TextMessages.FileIsTooBig, FriendlyConvert.BytesToString(_maxFileSize))
                   );
            }


            var filename = GenerateFileName() + Path.GetExtension(file.FileName);

            var fullPath = Path.Combine(_fileStorage, filename);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return filename;
        }


        private static string GenerateFileName()
        {
            return DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString(CultureInfo.InvariantCulture);
        }
    }
}