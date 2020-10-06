using Abstractions.IRepository;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Infrastructure.Repository
{
    public class LogRepository : ILogRepository
    {
        private readonly string _logDirectory;
        private readonly string _fileName = DateTime.Now.ToString("dd-MM-yyyy")+ ".log";

        public LogRepository(IConfiguration configuration)
        {
            _logDirectory = configuration.GetSection("Location:LogStorage").Get<string>();

            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }

        public void LogError(Exception exception)
        {
            //TODO: remove whole rep and replace it with some log4net or something
            //var value =
            //    $"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}] {Environment.NewLine}"
            //    +$"[{exception.GetType().Name}] {Environment.NewLine}"//mostly to differ regular exception from custom
            //    + JsonConvert.SerializeObject(exception, Formatting.Indented)
            //    + Environment.NewLine + Environment.NewLine
            //    + "----------------------------------------------------------"
            //    ;

            //File.WriteAllText(Path.Combine(_logDirectory, _fileName), value);
        }

    }
}
