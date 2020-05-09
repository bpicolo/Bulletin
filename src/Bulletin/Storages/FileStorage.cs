using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Storage.Net;
using Storage.Net.Blobs;

namespace Bulletin.Storages
{
    public class FileStorage : IStorage, IFileProvidingStorage
    {
        private readonly FileStorageOptions _options;
        private readonly IBlobStorage _storage;

        private FileStorage(FileStorageOptions options)
        {
            _options = options;
            _storage = StorageFactory.Blobs.DirectoryFiles(_options.Directory);
        }

        IBlobStorage IStorage.GetBlobStorage()
        {
            return _storage;
        }

        public IFileProvider GetFileProvider()
        {
            return new PhysicalFileProvider(_options.Directory);
        }

        public static FileStorage New(string directory, UrlOptions urlOptions)
        {
            if (!Path.IsPathRooted(directory))
            {
                directory = Path.GetFullPath(directory);
            }

            return new FileStorage(new FileStorageOptions
            {
                Directory = directory,
                UrlOptions = urlOptions
            });
        }

        public UrlOptions UrlOptions()
        {
            return _options.UrlOptions;
        }
    }
}