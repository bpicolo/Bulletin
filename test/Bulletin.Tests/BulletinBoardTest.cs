using System;
using System.Threading.Tasks;
using Bulletin.Models;
using Xunit;

namespace Bulletin.Tests
{
    public class BulletinBoardTest
    {
        private Attachment AttachmentStub()
        {
            return new Attachment()
            {
                Location = "hello.jpg"
            };
        }

        [Fact]
        public void TestAbsoluteUrlsGeneratedWithDefaultPrefix()
        {
            var board = BulletinBoardFactory.InMemory("", new UrlOptions());

            Assert.Equal(
                "https://localhost:5001/bulletin-static/hello.jpg",
                board.AbsoluteUrlFor(AttachmentStub()));
        }

        [Fact]
        public void TestHttpsUrlWithDefaultPortNotIncluded() {
            var board = BulletinBoardFactory.InMemory("", new UrlOptions
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
            var board = BulletinBoardFactory.InMemory("", new UrlOptions
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
            var board = BulletinBoardFactory.InMemory("", bulletinBoardOptions: new BulletinBoardOptions
            {
                RoutePrefix ="my-custom-prefix"
            });

            Assert.Equal(
                "https://localhost:5001/my-custom-prefix/hello.jpg",
                board.AbsoluteUrlFor(AttachmentStub()));
        }

        [Fact]
        public void TestAbsoluteUrlWithCustomEmptyPrefix()
        {
            var board = BulletinBoardFactory.InMemory("", bulletinBoardOptions: new BulletinBoardOptions
            {
                RoutePrefix = ""
            });

            Assert.Equal(
                "https://localhost:5001/hello.jpg",
                board.AbsoluteUrlFor(AttachmentStub()));
        }

        [Fact]
        public void TestAbsoluteUrlWithBoardName()
        {
            var board = BulletinBoardFactory.InMemory("board-name");

            Assert.Equal(
                "https://localhost:5001/bulletin-static/board-name/hello.jpg",
                board.AbsoluteUrlFor(AttachmentStub()));
        }
    }
}