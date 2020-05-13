using System;
using System.Diagnostics.CodeAnalysis;
using Bulletin.Storage;

namespace Bulletin
{
    public class BulletinBoardOptions
    {
        private string _pathPrefix = "";
        public IUrlGenerator UrlGenerator { get; set; } = null;
        public bool PresignedUrls { get; set; } = false;
        public int PresignedUrlExpires { get; set; } = 3600;

        public BulletinBoardOptions([NotNull]string name, [NotNull]IStorage storage)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Bulletin board name must not be null or empty");
            }

            Name = name;
            Storage = storage;
        }

        public IStorage Storage { get; }

        public string Name { get; }

        public string PathPrefix
        {
            get => _pathPrefix;
            set => _pathPrefix = value == null ?
                throw new ArgumentException("Path prefix cannot be null, use \"\"") :
                value.Trim('/');
        }
    }
}