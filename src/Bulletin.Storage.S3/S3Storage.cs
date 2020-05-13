using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Storage.Net;
using Storage.Net.Amazon.Aws.Blobs;
using Storage.Net.Blobs;

namespace Bulletin.Storage.S3
{
    public class S3Storage : IStorage
    {
        private static string S3_HOST_BASE = ".s3.amazonaws.com";

        private readonly IAwsS3BlobStorage _storage;
        private readonly S3StorageOptions _options;

        public S3Storage(S3StorageOptions options)
        {
            _options = options;
            _storage = (IAwsS3BlobStorage) StorageFactory.Blobs.AwsS3(
                options.AccessKeyId,
                options.SecretAccessKey,
                options.SessionToken,
                options.BucketName,
                options.Region,
                options.EndpointUrl);
        }

        public Task WriteAsync(string fullPath, Stream dataStream, bool append = false, CancellationToken cancellationToken = default)
        {
            return _storage.WriteAsync(fullPath, dataStream, append, cancellationToken);
        }

        public Task DeleteAsync(string path, CancellationToken cancellationToken = default)
        {
            return _storage.DeleteAsync(path, cancellationToken);
        }

        public IUrlGenerator DefaultUrlGenerator(UrlGenerationOptions options)
        {
            if (options.PresignedUrls)
            {
                throw new NotImplementedException();
            }

            if (_options.EndpointUrl != null)
            {
                return new PublicUrlGenerator(
                    "https",
                    _options.EndpointUrl,
                    -1,
                    _options.BucketName);
            }

            // We default to amazon virtual-hosted urls over https, which are the only
            // type supported for new bucket types
            return new PublicUrlGenerator(
                "https",
                $"{_options.BucketName}{S3_HOST_BASE}",
                -1);
        }
    }
}