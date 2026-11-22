using Microsoft.EntityFrameworkCore;
using Dal.Lot;

namespace Dal.Contexts;
public class LotsDbContext : DbContext
{
    public LotsDbContext(DbContextOptions<LotsDbContext> options) : base(options) { }
    public DbSet<LotEntity> LotEntity => Set<LotEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LotEntity>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.User)
            .WithMany(x => x.Lots)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

            entity.HasMany(x => x.TargetOffers)
            .WithOne(x => x.TargetLot)
            .HasForeignKey(x => x.TargetLotId);

            entity.HasMany(x => x.TargetedOffers)
            .WithOne(x => x.OfferLot)
            .HasForeignKey(x => x.OfferLotId);

            entity.HasOne(x => x.AiEstimatedWorth)
            .WithOne(x => x.Lot)
            .HasForeignKey<AiEstimatedWorthEntity>(x => x.LotId);

            entity.HasOne(x => x.Category)
            .WithMany(x => x.Lots)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

            entity.HasMany(x => x.Parameters)
            .WithMany(x => x.Lots);

            entity.HasMany(x => x.Images)
            .WithOne(x => x.Lot)
            .HasForeignKey(x => x.LotId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

            entity.Property(x => x.Title)
            .HasMaxLength(155)
            .IsRequired();

            entity.Property(x => x.Description)
            .HasMaxLength(2000);

            entity.Property(x => x.Type)
            .IsRequired();

            entity.Property(x => x.EstimatedValue);

            entity.Property(x => x.DesiredExchange)
            .HasMaxLength(500);

            entity.Property(x => x.Status)
            .IsRequired();

            entity.Property(x => x.CreatedAt)
            .HasConversion(x => x.ToUniversalTime(), x => x)
            .IsRequired();

            entity.Property(x => x.PublishedAt)
            .HasConversion(x => x.ToUniversalTime(), x => x);
        });

        base.OnModelCreating(modelBuilder);
    }

}
