using System.Threading;
using System.Threading.Tasks;
using Bulletin.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulletin.EFCore
{
    public interface IBulletinDbContext
    {
        public DbSet<Attachment> Attachments { get; }
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}