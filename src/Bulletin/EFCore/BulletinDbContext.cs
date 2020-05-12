using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Bulletin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Bulletin.EFCore
{
    public interface IBulletinDbContext
    {
        public DbSet<Attachment> Attachments { get; }
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        public EntityEntry Update([NotNull] object entity);
    }
}