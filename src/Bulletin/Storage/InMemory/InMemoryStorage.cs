using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Storage.Net;
using Storage.Net.Blobs;

namespace Bulletin.Storage.InMemory
{
    public class InMemoryStorage : IStorage
    {
        private readonly IBlobStorage _storage;
        private readonly InMemoryStorageOptions _options;

        public InMemoryStorage(InMemoryStorageOptions options)
        {
            _storage = StorageFactory.Blobs.InMemory();
            _options = options;
        }

        public IUrlGenerator DefaultUrlGenerator(UrlGenerationOptions options)
        {
            if (options.PresignedUrls)
            {
                throw new NotImplementedException();
            }

            return new PublicUrlGenerator(
                _options.Scheme,
                _options.Host,
                _options.Port,
                $"bulletin-static/{options.BulletinBoardName}");
        }

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

        public string AbsoluteUrlFor(string location)
        {
            return new UriBuilder(
                _options.Scheme,
                _options.Host,
                _options.Port,
                location).Uri.AbsoluteUri;
        }
    }
}