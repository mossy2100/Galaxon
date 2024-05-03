using Galaxon.Astronomy.SpaceCalendars.com.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Galaxon.Astronomy.SpaceCalendars.com;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Document> Documents => Set<Document>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

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
