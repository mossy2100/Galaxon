using Galaxon.Astronomy.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Galaxon.Astronomy.SpaceCalendars.com;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext(options)
{
    public DbSet<Document> Documents => Set<Document>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Document>()
            .HasOne(doc => doc.Folder)
            .WithMany(folder => folder.Documents)
            .HasForeignKey(doc => doc.FolderId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
