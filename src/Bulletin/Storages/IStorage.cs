using Microsoft.Extensions.FileProviders;
using Storage.Net.Blobs;

namespace Bulletin.Storages
{
    public interface IStorage
    {
        public UrlOptions UrlOptions();
        internal IBlobStorage GetBlobStorage();
    }
}