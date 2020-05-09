using Bulletin.Storages;

namespace Bulletin
{
    public class BulletinBoardOptions
    {
        private string _routePrefix = "bulletin-static";

        public string RoutePrefix
        {
            get => _routePrefix;
            set => _routePrefix = value.Trim('/');
        }
    }
}