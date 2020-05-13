namespace Bulletin.Storage.InMemory
{
    public class InMemoryStorageOptions : StorageOptions
    {
        public string Scheme { get; set; } = "https";
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = -1;
    }
}