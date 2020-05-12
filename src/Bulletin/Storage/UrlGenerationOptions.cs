namespace Bulletin.Storage
{
    public class UrlGenerationOptions
    {

        public string BulletinBoardName { get; }
        public bool PresignedUrls { get; }
        public int PresignedUrlExpires { get; }

        public UrlGenerationOptions(string bulletinBoardName, bool presignedUrls, int presignedUrlExpires)
        {
            BulletinBoardName = bulletinBoardName;
            PresignedUrls = presignedUrls;
            PresignedUrlExpires = presignedUrlExpires;
        }
    }
}