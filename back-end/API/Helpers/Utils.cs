using Microsoft.Extensions.Configuration;
using System;

namespace API.Helpers
{
    public static class Utils
    {
        public static string AppendUrlToName(IConfiguration config, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            return config.GetSection("Kestrel:Endpoints:Https:Url").Get<string>()
                  + "/" + config.GetSection("Location:FileStorage").Get<string>()
                  + "/" + name;
        }
    }
}
