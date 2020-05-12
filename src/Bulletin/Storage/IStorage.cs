using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Bulletin.Storage
{
    public interface IStorage
    {
        public Task WriteAsync(
            string fullPath,
            Stream dataStream,
            bool append = false,
            CancellationToken cancellationToken = default);

        public Task DeleteAsync(
            string path,
            CancellationToken cancellationToken = default);

        public IUrlGenerator DefaultUrlGenerator(UrlGenerationOptions options);
    }
}