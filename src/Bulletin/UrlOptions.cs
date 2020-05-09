using System;

namespace Bulletin
{
    public class UrlOptions
    {
        public string Scheme { get; set; } = "https";
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 5001;

        public string GetAbsoluteUrl(string path)
        {
            return new UriBuilder(Scheme, Host, Port, path).Uri.AbsoluteUri;
        }
    }
}