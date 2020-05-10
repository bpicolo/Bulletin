using System;
using System.IO;
using System.Threading.Tasks;
using Bulletin.EFCore;
using Bulletin.Models;
using Bulletin.Storages;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Bulletin.Tests
{
    public class BulletinBoardTest
    {

        private readonly Mock<IBulletinDbContext> _mockContext;
        private readonly Mock<DbSet<Attachment>> _mockAttachments;
        private readonly Mock<IStorage> _mockStorage;

        public BulletinBoardTest()
        {
            _mockContext = new Mock<IBulletinDbContext>();
            _mockAttachments = new Mock<DbSet<Attachment>>();
            _mockStorage = new Mock<IStorage>();
            _mockContext.SetupGet(x => x.Attachments).Returns(_mockAttachments.Object);
        }

        [Fact]
        public async Task AttachmentSavesBasicData()
        {
            var file = File.OpenRead("./resources/text.txt");
            var board = GetBoard("board-name");

            var attachment = await board.AttachAsync(file);

            Assert.Equal("board-name", attachment.Board);
            Assert.Equal("text/plain", attachment.ContentType);
            Assert.Equal("text.txt", attachment.OriginalFilename);
            Assert.IsType<DateTime>(attachment.CreatedAt);
            Assert.Equal(9, attachment.SizeInBytes);
            Assert.Null(attachment.Metadata);
        }

        [Fact]
        public async Task AttachSavesToTheDatabase()
        {
            var file = File.OpenRead("./resources/text.txt");
            var board = GetBoard("");

            var attachment = await board.AttachAsync(file);

            _mockAttachments.Verify(m => m.AddAsync(attachment, default), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task AttachmentIsWrittenToStorage()
        {
            var file = File.OpenRead("./resources/text.txt");
            var board = GetBoard("");

            var attachment = await board.AttachAsync(file);

            _mockStorage.Verify(m => m.WriteAsync(
                It.IsAny<string>(),
                file,
                false,
                default), Times.Once);
        }

        [Fact]
        public async Task AssertShaSumGeneratedForAttachment()
        {
            var file = File.OpenRead("./resources/text.txt");
            var board = GetBoard("");

            var attachment = await board.AttachAsync(file);

            Assert.Equal(
                "a7939b43166ec185d6513aba3b11776eeb7d2e185dd30e1bd38e080af689043f",
                attachment.Checksum);
        }

        [Fact]
        public void AbsoluteUrlsGeneratedWithDefaultPrefix()
        {
            var board = GetBoard("");

            Assert.Equal(
                "https://localhost:5001/bulletin-static/hello.jpg",
                board.AbsoluteUrlFor(AttachmentStub()));
        }

        [Fact]
        public void HttpsUrlDoesntIncludeDefaultPort() {
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
        public void HttpUrlDoesntIncludeDefaultPort() {
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
        public void AbsoluteUrlUsesCustomPrefix()
        {
            var board = GetBoard("", routePrefix: "my-custom-prefix");

            Assert.Equal(
                "https://localhost:5001/my-custom-prefix/hello.jpg",
                board.AbsoluteUrlFor(AttachmentStub()));
        }

        [Fact]
        public void AbsoluteUrlIgnoresEmptyPrefix()
        {
            var board = GetBoard("", "");

            Assert.Equal(
                "https://localhost:5001/hello.jpg",
                board.AbsoluteUrlFor(AttachmentStub()));
        }

        [Fact]
        public void AbsoluteUrlRendersBoardName()
        {
            var board = GetBoard("board-name");

            Assert.Equal(
                "https://localhost:5001/bulletin-static/board-name/hello.jpg",
                board.AbsoluteUrlFor(AttachmentStub()));
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
            _mockStorage.Setup(v => v.UrlOptions()).Returns(urlOptions);

            var options = new BulletinBoardOptions(name, _mockStorage.Object);

            if (routePrefix != null)
            {
                options.RoutePrefix = routePrefix;
            }

            return new BulletinBoard(_mockContext.Object, options);
        }
    }
}