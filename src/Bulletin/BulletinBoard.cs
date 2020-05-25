using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Bulletin.EFCore;
using Bulletin.Models;
using Bulletin.Storage;
using Microsoft.Extensions.FileProviders;

namespace Bulletin
{
    public class BulletinBoard : IBulletinBoard
    {
        private readonly IBulletinDbContext _dbContext;
        private readonly BulletinBoardOptions _options;
        private readonly IUrlGenerator _urlGenerator;

        public BulletinBoard(IBulletinDbContext dbContext, BulletinBoardOptions options)
        {
            _options = options;
            _dbContext = dbContext;
            _urlGenerator = GetUrlGenerator(_options);
        }

        public string AbsoluteUrlFor(Attachment attachment)
        {
            return _urlGenerator.AbsoluteUrlFor(attachment.Location);
        }

        public string GetName()
        {
            return _options.Name;
        }

        public IFileProvider GetFileProvider()
        {
            return _options.Storage.GetFileProvider();
        }

        public async Task<Attachment> AttachAsync(string filename, Stream stream)
        {
            var sum = ShaSum(stream);
            var size = stream.Length;
            stream.Seek(0, SeekOrigin.Begin);

            var ext = Path.GetExtension(filename);
            var mimetype = MimeMapping.MimeUtility.GetMimeMapping(ext);

            var destination = PathNameGenerator.GenerateUniquePathName(ext);
            await _options.Storage.WriteAsync(destination, stream);

            var attachment = new Attachment
            {
                Board = _options.Name,
                Location = destination,
                ContentType = mimetype,
                OriginalFilename = Path.GetFileName(filename),
                Checksum = sum,
                CreatedAt = new DateTime(),
                SizeInBytes = size,
                Metadata = null
            };

            await _dbContext.Attachments.AddAsync(attachment);
            await _dbContext.SaveChangesAsync();

            return attachment;
        }

        public async Task<Attachment> AttachAsync(FileStream file)
        {
            return await AttachAsync(file.Name, file);
        }

        public async Task DeleteAsync(Attachment attachment)
        {
            await _options.Storage.DeleteAsync(attachment.Location);
            attachment.DeletedAt = DateTime.Now;
            _dbContext.Update(attachment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Stream> DownloadAsync(Attachment attachment)
        {
            return await _options.Storage.ReadAsync(attachment.Location);
        }

        private static string ShaSum(Stream data)
        {
            StringBuilder sb = new StringBuilder();

            data.Seek(0, SeekOrigin.Begin);
            using var shaSum = SHA256.Create();
            var sum = shaSum.ComputeHash(data);
            foreach (var b in sum)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        private static IUrlGenerator GetUrlGenerator(BulletinBoardOptions options)
        {
            if (options.UrlGenerator != null)
            {
                return options.UrlGenerator;
            }

            return options.Storage.DefaultUrlGenerator(
                new UrlGenerationOptions(
                    options.Name,
                    options.PresignedUrls,
                    options.PresignedUrlExpires));
        }
    }
}