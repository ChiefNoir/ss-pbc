namespace SSPBC.Helpers
{
    public static class Utils
    {
        public static string AppendUrlToName(IConfiguration configuration, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            return $"{configuration["Endpoint"]}{configuration["Location:StaticFilesRequestPath"]}/{name}";
        }

        public static void CheckFileStorageDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
