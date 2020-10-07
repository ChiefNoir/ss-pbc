using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
    public static class Utils
    {
        public static string AppendUrlToName(IConfiguration config, string name)
        {
            return config.GetSection("Kestrel:Endpoints:Https:Url").Get<string>()
                  + "/" + config.GetSection("Location:FileStorage").Get<string>()
                  + "/" + name;
        }
    }
}
