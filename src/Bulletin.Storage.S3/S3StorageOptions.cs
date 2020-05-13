using Amazon;

namespace Bulletin.Storage.S3
{
    public class S3StorageOptions
    {
        public string AccessKeyId { get; }
        public string SecretAccessKey { get; }
        public string SessionToken { get; }
        public string BucketName { get; }
        public string EndpointUrl { get;  } = null;
        public string Region { get; }

        public S3StorageOptions(
            string bucketName,
            string accessKeyId,
            string secretAccessKey,
            string region,
            string sessionToken = null,
            string endpointUrl = null)
        {
            AccessKeyId = accessKeyId;
            SecretAccessKey = secretAccessKey;
            SessionToken = sessionToken;
            BucketName = bucketName;
            // Until proven otherwise, I'm assuming every S3-compatible cloud provider
            // supports https. Library internals dictate a protocol be provided,
            // and many folk may just copy paste the hostname provided by the provider
            EndpointUrl = endpointUrl != null && endpointUrl.StartsWith("http") ? endpointUrl : $"https://{endpointUrl}";
            Region = region;
        }
    }
}