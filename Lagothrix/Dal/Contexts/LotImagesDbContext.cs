using Dal.LotImage;
using Microsoft.EntityFrameworkCore;

namespace Dal.Contexts;
public class LotImagesDbContext : DbContext
{
    public LotImagesDbContext(DbContextOptions<LotImagesDbContext> options) : base(options) { }
    public DbSet<LotImageEntity> LotImageEntity => Set<LotImageEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LotImageEntity>(entity =>
        {
            entity.HasKey(entity => entity.Id);

            entity.HasOne(x => x.Lot)
            .WithMany(x => x.Images)
            .HasForeignKey(x => x.LotId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

            entity.Property(x => x.Url)
            .IsRequired();

            entity.Property(x => x.SortOrder)
            .IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}
