namespace Bulletin.Storages
{
    public class S3StorageOptions
    {
        private string AccessKeyId { get; }
        private string SecretAccessKey { get; }
        private string SessionToken { get; }
        private string BucketName { get; }
        private string Region { get; } = "";
        public string EndpointUrl { get; set; } = null;

        public S3StorageOptions(
            string accessKeyId,
            string secretAccessKey,
            string sessionToken,
            string bucketName)
        {
            AccessKeyId = accessKeyId;
            SecretAccessKey = secretAccessKey;
            SessionToken = sessionToken;
            BucketName = bucketName;
        }

        // public static S3StorageOptions FromCredentialsFile(
        //     string profileName,
        //     string bucketName,
        //     string region)
        // {
        //
        // }
    }
}