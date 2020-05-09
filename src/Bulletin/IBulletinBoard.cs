using System;
using System.IO;
using System.Threading.Tasks;
using Bulletin.Models;
using Microsoft.Extensions.FileProviders;

namespace Bulletin
{
    public interface IBulletinBoard
    {
        public Task<Attachment> AttachAsync(FileStream file);
        // public IFileProvider GetFileProvider();
        public string AbsoluteUrlFor(Attachment attachment);
    }
}