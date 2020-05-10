using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Storage.Net.Blobs;

namespace Bulletin.Storages
{
    public interface IStorage
    {
        public UrlOptions UrlOptions();

        public Task WriteAsync(
            string fullPath,
            Stream dataStream,
            bool append = false,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}