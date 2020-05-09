using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Storage.Net.Blobs;

namespace Bulletin
{
    class Uploader
    {
        private readonly IBlobStorage _adapter;
        private readonly RNGCryptoServiceProvider _provider;

        public Uploader(string name, IBlobStorage adapter)
        {
            _adapter = adapter;
            _provider = new RNGCryptoServiceProvider();
        }

        public async Task<UploadedFile> Upload(FileStream fs)
        {
            var ext = Path.GetExtension(fs.Name);
            string destination = GenerateUniqueFilepath(ext);
            await _adapter.WriteAsync(destination, fs);
            return new UploadedFile(destination, ext);
        }

        private string GenerateUniqueFilepath(string ext)
        {
            var random = Guid.NewGuid().ToString();

            return Path.Combine(
                random.Substring(0, 2), random.Substring(2, 2),
                $"{random}{ext}");
        }
    }
}
