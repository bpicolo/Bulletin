using Microsoft.Extensions.FileProviders;

namespace Bulletin.Storages
{
    public interface IFileProvidingStorage
    {
        public IFileProvider GetFileProvider();
    }
}