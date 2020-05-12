using Bulletin.Storage;
using Bulletin.Storage.File;
using Xunit;

namespace Bulletin.Tests.Storage.File
{
    public class FileStorageTest
    {

        private static string GetDefaultUrl(FileStorage storage, string location)
        {
            return storage.DefaultUrlGenerator(
                new UrlGenerationOptions("boardname", false, 3600)
            ).AbsoluteUrlFor(location);
        }

        [Fact]
        public void UrlGeneratorHttpsUrlDoesntIncludeDefaultPort() {
            var storage = new FileStorage(
                new FileStorageOptions("/tmp")
                {
                    Scheme = "https",
                    Port = 443
                });

            Assert.Equal(
                "https://localhost/bulletin-static/boardname/hello.jpg",
                GetDefaultUrl(storage, "hello.jpg"));
        }

        [Fact]
        public void UrlGeneratorHttpUrlDoesntIncludeDefaultPort() {
            var storage = new FileStorage(
                new FileStorageOptions("/tmp")
                {
                    Scheme = "http",
                    Port = 80
                });

            Assert.Equal(
                "http://localhost/bulletin-static/boardname/hello.jpg",
                GetDefaultUrl(storage, "hello.jpg"));
        }

        [Fact]
        public void UrlGeneratorUrlWithNonstandardPort() {
            var storage = new FileStorage(
                new FileStorageOptions("/tmp")
                {
                    Scheme = "http",
                    Port = 81
                });

            Assert.Equal(
                "http://localhost:81/bulletin-static/boardname/hello.jpg",
                GetDefaultUrl(storage, "hello.jpg"));
        }
    }
}