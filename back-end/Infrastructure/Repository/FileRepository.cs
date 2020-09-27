using Abstractions.Exceptions;
using Abstractions.IRepository;
using Common.FriendlyConverters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Infrastructure.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly IConfiguration _configuration;


        public FileRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Save(IFormFile file)
        {
            var maxFileSize = _configuration.GetSection("Location:MaxFileSizeBytes").Get<int>();

            if (file == null || file.Length == 0)
            {
                throw new InfrastructureException(Resources.TextMessages.FileIsEmpty);
            }

            if (file.Length > maxFileSize)
            {
                throw new InfrastructureException
                   (
                        string.Format(Resources.TextMessages.FileIsTooBig, FriendlyConvert.BytesToString(maxFileSize))
                   );
            }

            var folderName = _configuration.GetSection("Location:FileStorage").Get<string>();
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            var filename = GenerateFileName() + Path.GetExtension(file.FileName);

            var fullPath = Path.Combine(pathToSave, filename);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return filename;
        }


        private static string GenerateFileName()
        {
            return DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString();
        }
    }
}