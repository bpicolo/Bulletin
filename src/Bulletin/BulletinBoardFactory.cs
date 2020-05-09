using Bulletin.Storages;

namespace Bulletin
{
    public class BulletinBoardFactory
    {

        public static BulletinBoard InMemory(
            string name,
            UrlOptions urlOptions = null,
            BulletinBoardOptions bulletinBoardOptions = null)
        {
            urlOptions ??= new UrlOptions();
            bulletinBoardOptions ??= new BulletinBoardOptions();

            return new BulletinBoard(name, new InMemoryStorage(urlOptions), bulletinBoardOptions);
        }
    }
}