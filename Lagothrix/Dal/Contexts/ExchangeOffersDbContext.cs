using Dal.ExchangeOffer;
using Microsoft.EntityFrameworkCore;

namespace Dal.Contexts;
public class ExchangeOffersDbContext : DbContext
{
    public ExchangeOffersDbContext(DbContextOptions<ExchangeOffersDbContext> options) : base(options) { }
    public DbSet<ExchangeOfferEntity> ExchangeOfferEntity => Set<ExchangeOfferEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExchangeOfferEntity>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.TargetLot)
            .WithMany(x => x.TargetOffers)
            .HasForeignKey(x => x.TargetLotId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.OfferLot)
            .WithMany(x => x.TargetedOffers)
            .HasForeignKey(x => x.OfferLotId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Sender)
            .WithMany(x => x.SentOffers)
            .HasForeignKey(x => x.SenderId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Receiver)
            .WithMany(x => x.ReceivedOffers)
            .HasForeignKey(x => x.ReceiverId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

            entity.Property(x => x.Status)
            .IsRequired();

            entity.Property(x => x.CreatedAt)
            .HasConversion(x => x.ToUniversalTime(), x => x)
            .IsRequired();

            entity.Property(x => x.UpdatedAt)
            .HasConversion(x => x.ToUniversalTime(), x => x)
            .IsRequired();
        });
    }
}
