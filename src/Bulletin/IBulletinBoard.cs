using System;
using System.IO;
using System.Threading.Tasks;
using Bulletin.Models;
using Microsoft.Extensions.FileProviders;

namespace Bulletin
{
    public interface IBulletinBoard
    {
        public Task<Attachment> AttachAsync(string filename, Stream stream);
        public Task<Attachment> AttachAsync(FileStream file);
        public Task DeleteAsync(Attachment attachment);
        public Task<Stream> DownloadAsync(Attachment attachment);
        public string AbsoluteUrlFor(Attachment attachment);
        public string GetName();
        public IFileProvider GetFileProvider();
    }
}