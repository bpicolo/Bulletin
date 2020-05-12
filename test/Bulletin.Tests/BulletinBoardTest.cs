using System;
using System.IO;
using System.Threading.Tasks;
using Bulletin.EFCore;
using Bulletin.Models;
using Bulletin.Storage;
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
            var board = GetBoard();

            var attachment = await board.AttachAsync(file);

            Assert.Equal("test-board", attachment.Board);
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
            var board = GetBoard();

            var attachment = await board.AttachAsync(file);

            _mockAttachments.Verify(m => m.AddAsync(attachment, default), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task AttachmentIsWrittenToStorage()
        {
            var file = File.OpenRead("./resources/text.txt");
            var board = GetBoard();

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
            var board = GetBoard();

            var attachment = await board.AttachAsync(file);

            Assert.Equal(
                "a7939b43166ec185d6513aba3b11776eeb7d2e185dd30e1bd38e080af689043f",
                attachment.Checksum);
        }

        [Fact]
        public void UsesACustomUrlGeneratorIfProvided()
        {
            var generator = new Mock<IUrlGenerator>();
            var attachment = AttachmentStub();
            generator.Setup(g => g.AbsoluteUrlFor(attachment.Location)).Returns(
                "http://test.localhost/path.jpg");

            var board = GetBoard(o => o.UrlGenerator = generator.Object);

            Assert.Equal("http://test.localhost/path.jpg", board.AbsoluteUrlFor(attachment));
        }

        private Attachment AttachmentStub()
        {
            return new Attachment()
            {
                Location = "hello.jpg"
            };
        }

        private BulletinBoard GetBoard(Action<BulletinBoardOptions> optionsAction = null)
        {
            var options = new BulletinBoardOptions("test-board", _mockStorage.Object);
            optionsAction?.Invoke(options);
            return new BulletinBoard(_mockContext.Object, options);
        }
    }
}