using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Bulletin.Models;
using Bulletin.Storages;
using Microsoft.Extensions.FileProviders;

namespace Bulletin
{
    public class BulletinBoard : IBulletinBoard
    {
        private readonly string _name;
        private readonly string _routePrefix;
        private readonly Uploader _uploader;
        private readonly BulletinBoardOptions _options;
        private readonly IStorage _storage;

        public BulletinBoard(string name, IStorage storage, BulletinBoardOptions options = null)
        {
            options ??= new BulletinBoardOptions();

            _name = name;
            _options = options;
            _storage = storage;
            _uploader = new Uploader(_name, storage.GetBlobStorage());
            _routePrefix = options.RoutePrefix != null ? $"/{options.RoutePrefix}/{_name}".TrimEnd('/') : $"/bulletin-static/{_name}".TrimEnd('/');
        }

        public string AbsoluteUrlFor(Attachment attachment)
        {
            return _storage.UrlOptions().GetAbsoluteUrl($"{_routePrefix}/{attachment.Location}");
        }

        public async Task<Attachment> AttachAsync(FileStream file)
        {
            var upload = await _uploader.Upload(file);
            var mimetype = MimeMapping.MimeUtility.GetMimeMapping(upload.Extension);

            return new Attachment()
            {
                Board = _name,
                Location = upload.Location,
                ContentType = mimetype,
                Filename = Path.GetFileName(file.Name),
                Checksum = ShaSum(file),
                CreatedAt = new DateTime(),
                SizeInBytes = file.Length,
                Metadata = null
            };
        }

        private static string ShaSum(FileStream file)
        {
            StringBuilder sb = new StringBuilder();

            file.Seek(0, SeekOrigin.Begin);
            using var shaSum = SHA256.Create();
            var sum = shaSum.ComputeHash(file);
            foreach (var b in sum)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}