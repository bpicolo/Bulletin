using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Storage.Net;
using Storage.Net.Blobs;

namespace Bulletin.Storages
{
    public class InMemoryStorage : IStorage
    {
        private readonly IBlobStorage _storage;
        private readonly UrlOptions _urlOptions;

        public InMemoryStorage(UrlOptions urlOptions)
        {
            _urlOptions = urlOptions;
            _storage = StorageFactory.Blobs.InMemory();
        }

        public UrlOptions UrlOptions() => _urlOptions;

        public Task WriteAsync(
            string fullPath,
            Stream dataStream,
            bool append = false,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return _storage.WriteAsync(fullPath, dataStream, append, cancellationToken);
        }

        public Task DeleteAsync(string path, CancellationToken cancellationToken = default)
        {
            return _storage.DeleteAsync(path, cancellationToken);
        }
    }
}