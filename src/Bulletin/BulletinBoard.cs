using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Bulletin.EFCore;
using Bulletin.Models;
using Bulletin.Storages;
using Microsoft.Extensions.FileProviders;

namespace Bulletin
{
    public class BulletinBoard : IBulletinBoard
    {
        private readonly string _routePrefix;
        private readonly IBulletinDbContext _dbContext;
        private readonly Uploader _uploader;
        private readonly BulletinBoardOptions _options;

        public BulletinBoard(IBulletinDbContext dbContext, BulletinBoardOptions options)
        {

            _options = options;
            _dbContext = dbContext;
            _uploader = new Uploader(options.Name, options.Storage.GetBlobStorage());
            _routePrefix = options.RoutePrefix != null ?
                $"/{options.RoutePrefix}/{options.Name}".TrimEnd('/') :
                $"/bulletin-static/{options.Name}".TrimEnd('/');
        }

        public string AbsoluteUrlFor(Attachment attachment)
        {
            return _options.Storage.UrlOptions().GetAbsoluteUrl($"{_routePrefix}/{attachment.Location}");
        }

        public async Task<Attachment> AttachAsync(FileStream file)
        {
            var upload = await _uploader.Upload(file);
            var mimetype = MimeMapping.MimeUtility.GetMimeMapping(upload.Extension);

            var attachment = new Attachment()
            {
                Board = _options.Name,
                Location = upload.Location,
                ContentType = mimetype,
                Filename = Path.GetFileName(file.Name),
                Checksum = ShaSum(file),
                CreatedAt = new DateTime(),
                SizeInBytes = file.Length,
                Metadata = null
            };
            await _dbContext.Attachments.AddAsync(attachment);
            await _dbContext.SaveChangesAsync();

            return attachment;
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