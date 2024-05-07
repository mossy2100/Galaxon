using Galaxon.Astronomy.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Galaxon.Astronomy.SpaceCalendars.com;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext(options)
{
    public DbSet<DocumentRecord> Documents => Set<DocumentRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DocumentRecord>()
            .HasOne(doc => doc.Parent)
            .WithMany(folder => folder.Children)
            .HasForeignKey(doc => doc.ParentId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
