using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Bulletin.EFCore;
using Bulletin.Models;
using Bulletin.Storages;
using Moq;
using Xunit;

namespace Bulletin.Tests
{
    public class BulletinBoardTest
    {

        private readonly Mock<IBulletinDbContext> _mockContext;

        public BulletinBoardTest()
        {
            _mockContext = new Mock<IBulletinDbContext>();
        }

        private Attachment AttachmentStub()
        {
            return new Attachment()
            {
                Location = "hello.jpg"
            };
        }


        private BulletinBoard GetBoard(
            string name,
            string routePrefix = null,
            UrlOptions urlOptions = null)
        {
            urlOptions ??= new UrlOptions();

            var options = new BulletinBoardOptions(
                name,
                new InMemoryStorage(urlOptions));

            if (routePrefix != null)
            {
                options.RoutePrefix = routePrefix;
            }

            return new BulletinBoard(_mockContext.Object, options);
        }

        [Fact]
        public void TestAbsoluteUrlsGeneratedWithDefaultPrefix()
        {
            var board = GetBoard("");

            Assert.Equal(
                "https://localhost:5001/bulletin-static/hello.jpg",
                board.AbsoluteUrlFor(AttachmentStub()));
        }

        [Fact]
        public void TestHttpsUrlWithDefaultPortNotIncluded() {
            var board = GetBoard("", urlOptions: new UrlOptions
            {
                Scheme = "https",
                Port = 443
            });

            Assert.Equal(
                "https://localhost/bulletin-static/hello.jpg",
                board.AbsoluteUrlFor(AttachmentStub()));
        }

        [Fact]
        public void TestHttpUrlWithDefaultPortNotIncluded() {
            var board = GetBoard("", urlOptions: new UrlOptions
            {
                Scheme = "http",
                Port = 80
            });

            Assert.Equal(
                "http://localhost/bulletin-static/hello.jpg",
                board.AbsoluteUrlFor(AttachmentStub()));
        }

        [Fact]
        public void TestAbsoluteUrlWithCustomPrefix()
        {
            var board = GetBoard("", routePrefix: "my-custom-prefix");

            Assert.Equal(
                "https://localhost:5001/my-custom-prefix/hello.jpg",
                board.AbsoluteUrlFor(AttachmentStub()));
        }

        [Fact]
        public void TestAbsoluteUrlWithCustomEmptyPrefix()
        {
            var board = GetBoard("", "");

            Assert.Equal(
                "https://localhost:5001/hello.jpg",
                board.AbsoluteUrlFor(AttachmentStub()));
        }

        [Fact]
        public void TestAbsoluteUrlWithBoardName()
        {
            var board = GetBoard("board-name");

            Assert.Equal(
                "https://localhost:5001/bulletin-static/board-name/hello.jpg",
                board.AbsoluteUrlFor(AttachmentStub()));
        }
    }
}