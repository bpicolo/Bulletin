using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Bulletin.EFCore;
using Bulletin.Models;

namespace Bulletin
{
    public class BulletinBoard : IBulletinBoard
    {
        private readonly string _routePrefix;
        private readonly IBulletinDbContext _dbContext;
        private readonly BulletinBoardOptions _options;

        public BulletinBoard(IBulletinDbContext dbContext, BulletinBoardOptions options)
        {

            _options = options;
            _dbContext = dbContext;
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
            var ext = Path.GetExtension(file.Name);
            var mimetype = MimeMapping.MimeUtility.GetMimeMapping(ext);

            var destination = GenerateUniqueStorageName(ext);
            await _options.Storage.WriteAsync(destination, file);

            var attachment = new Attachment
            {
                Board = _options.Name,
                Location = destination,
                ContentType = mimetype,
                OriginalFilename = Path.GetFileName(file.Name),
                Checksum = ShaSum(file),
                CreatedAt = new DateTime(),
                SizeInBytes = file.Length,
                Metadata = null
            };

            await _dbContext.Attachments.AddAsync(attachment);
            await _dbContext.SaveChangesAsync();

            return attachment;
        }

        public async Task DeleteAsync(Attachment attachment)
        {
            await _options.Storage.DeleteAsync(attachment.Location);
            attachment.DeletedAt = DateTime.Now;
            _dbContext.Update(attachment);
            await _dbContext.SaveChangesAsync();
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

        private static string GenerateUniqueStorageName(string ext)
        {
            var random = Guid.NewGuid().ToString();

            return Path.Combine(
                random.Substring(0, 2), random.Substring(2, 2),
                $"{random}{ext}");
        }
    }
}