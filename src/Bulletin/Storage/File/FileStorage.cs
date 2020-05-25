using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Storage.Net;
using Storage.Net.Blobs;

namespace Bulletin.Storage.File
{
    public class FileStorage : IStorage
    {
        private readonly FileStorageOptions _options;
        private readonly IBlobStorage _storage;

        public FileStorage(FileStorageOptions options)
        {
            _options = options;
            _storage = StorageFactory.Blobs.DirectoryFiles(_options.Directory);
        }

        public async Task<Stream> ReadAsync(string path, CancellationToken cancellationToken = default)
        {
            return await _storage.OpenReadAsync(path, cancellationToken);
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
            CancellationToken cancellationToken = default)
        {
            return _storage.WriteAsync(fullPath, dataStream, append, cancellationToken);
        }

        public Task DeleteAsync(string path, CancellationToken cancellationToken = default)
        {
            return _storage.DeleteAsync(path, cancellationToken);
        }

        public IFileProvider GetFileProvider()
        {
            var path = _options.Directory;
            if (!Path.IsPathRooted(path))
            {
                path = Path.GetFullPath(path);
            }
            return new PhysicalFileProvider(path);
        }
    }
}