using Abstractions.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly IConfiguration _configuration;

        public FileRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> Save(IFormFile file)
        {
            if (file == null || file.Length == 0) //TODO: max file size
                throw new Exception("Empty file"); //TODO: custom exception

            var folderName = _configuration.GetSection("Location").GetValue<string>("FileStorage");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);


            var filename = GenerateFileName() + Path.GetExtension(file.FileName);

            var fullPath = Path.Combine(pathToSave, filename);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filename;
        }


        private static string GenerateFileName()
        {
            //TODO: re-do
            return DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString();
        }
    }
}
