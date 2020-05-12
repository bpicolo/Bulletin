namespace Bulletin.Storage.File
{
    public class FileStorageOptions : StorageOptions
    {
        public FileStorageOptions(string directory)
        {
            Directory = directory;
        }

        public string Directory { get; }

        public string Scheme { get; set; } = "https";
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 5001;
    }
}