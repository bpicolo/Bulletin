using Microsoft.Extensions.FileProviders;
using Storage.Net;
using Storage.Net.Blobs;

namespace Bulletin.Storages
{
    public class InMemoryStorage : IStorage
    {
        private readonly IBlobStorage _storage;
        private readonly UrlOptions _urlOptions;

        internal InMemoryStorage(UrlOptions urlOptions)
        {
            _urlOptions = urlOptions;
            _storage = StorageFactory.Blobs.InMemory();
        }

        public UrlOptions UrlOptions() => _urlOptions;

        IBlobStorage IStorage.GetBlobStorage() => _storage;
    }
}