using System;

namespace Bulletin.Storage
{
    internal class LocalUrlGenerator : IUrlGenerator
    {
        private readonly string _scheme;
        private readonly string _host;
        private readonly int _port;
        private readonly string _pathPrefix;

        public LocalUrlGenerator(string scheme, string host, int port, string pathPrefix)
        {
            _scheme = scheme;
            _host = host;
            _port = port;
            _pathPrefix = pathPrefix;
        }


        public string AbsoluteUrlFor(string location)
        {
            if (location.StartsWith('/'))
            {
                location = location.TrimStart('/');
            }

            return new UriBuilder(
                _scheme,
                _host,
                _port,
                _pathPrefix != null ? $"/{_pathPrefix}/{location}" : $"/{location}").Uri.AbsoluteUri;
        }
    }
}