using System;
using Bulletin.Storages;

namespace Bulletin
{
    public class BulletinBoardOptions
    {
        private string _routePrefix = "bulletin-static";

        private readonly IStorage _storage;
        private readonly string _name;

        public BulletinBoardOptions(String name, IStorage storage)
        {
            _name = name;
            _storage = storage;
        }

        public IStorage Storage
        {
            get => _storage;
        }
        public string Name
        {
            get => _name;
        }

        public string RoutePrefix
        {
            get => _routePrefix;
            set => _routePrefix = value == null ?
                throw new ArgumentException("Route prefix cannot be null, use \"\"") :
                value.Trim('/');
        }
    }
}