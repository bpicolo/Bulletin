namespace Bulletin.Storages
{
    public class FileStorageOptions
    {
        internal string Directory { get; set; }
        internal UrlOptions UrlOptions { get; set; } = new UrlOptions();
    }
}