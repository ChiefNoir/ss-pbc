namespace SSPBC.Helpers
{
    public static class Utils
    {
        public static void CheckFileStorageDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
