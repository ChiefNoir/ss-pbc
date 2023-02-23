namespace SSPBC.Admin.Helpers
{
    public static class Utils
    {
        public static string AppendUrlToName(IConfiguration configuration, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return $"{configuration["PublicEndpoint"]}{configuration["Location:StaticFilesRequestPath"]}/{name}";
        }
    }
}
