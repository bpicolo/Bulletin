using Bulletin.Storage;
using Bulletin.Storage.InMemory;
using Xunit;

namespace Bulletin.Tests.Storage.InMemory
{
    public class InMemoryStorageTest
    {
        private static string GetDefaultUrl(InMemoryStorage storage, string location)
        {
            return storage.DefaultUrlGenerator(
                new UrlGenerationOptions("boardname", false, 3600)
            ).AbsoluteUrlFor(location);
        }

        [Fact]
        public void UrlGeneratorHttpsUrlDoesntIncludeDefaultPort() {
            var storage = new InMemoryStorage(
                new InMemoryStorageOptions
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
            var storage = new InMemoryStorage(
                new InMemoryStorageOptions
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
            var storage = new InMemoryStorage(
                new InMemoryStorageOptions
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