using Dal.Entities;
using Dal.Entities.ExchangeOffer;
using Dal.Entities.Lot;
using Dal.Entities.LotImage;
using Microsoft.EntityFrameworkCore;

namespace Dal;
public class NpgsqlDbContext : DbContext
{
    public NpgsqlDbContext(DbContextOptions<NpgsqlDbContext> options) : base(options) { }
    public DbSet<ExchangeOfferEntity> ExchangeOfferEntity => Set<ExchangeOfferEntity>();
    public DbSet<LotImageEntity> LotImageEntity => Set<LotImageEntity>();
    public DbSet<LotEntity> LotEntity => Set<LotEntity>();
    public DbSet<ParameterEntity> ParameterEntity => Set<ParameterEntity>();
    public DbSet<AiEstimatedValueEntity> AiEstimatesEntity => Set<AiEstimatedValueEntity>();
    public DbSet<CategoryEntity> CategoryEntity => Set<CategoryEntity>();
    public DbSet<ParameterValueEntity> ParameterValueEntity => Set<ParameterValueEntity>();
    public DbSet<UserEntity> UserEntity => Set<UserEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasMany(x => x.Lots)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(x => x.ReceivedOffers)
            .WithOne(x => x.Receiver)
            .HasForeignKey(x => x.ReceiverId)
            .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(x => x.SentOffers)
            .WithOne(x => x.Sender)
            .HasForeignKey(x => x.SenderId)
            .OnDelete(DeleteBehavior.Cascade);

            entity.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

            entity.Property(x => x.Email)
            .HasMaxLength(255)
            .IsRequired();

            entity.Property(x => x.PasswordHash)
            .IsRequired();

            entity.Property(x => x.CreatedAt)
            .HasConversion(x => x.ToUniversalTime(), x => x)
            .IsRequired();
        });

        modelBuilder.Entity<ParameterValueEntity>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.Parameter)
            .WithMany(x => x.Values)
            .HasForeignKey(x => x.ParameterId)
            .IsRequired();

            entity.Property(x => x.Value)
            .IsRequired();
        });

        modelBuilder.Entity<CategoryEntity>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasMany(x => x.Lots)
            .WithOne(x => x.Category)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

            entity.Property(x => x.Name);
        });

        modelBuilder.Entity<AiEstimatedValueEntity>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.Lot)
            .WithOne(x => x.AiEstimatedValue)
            .HasForeignKey<AiEstimatedValueEntity>(x => x.LotId);
        });

        modelBuilder.Entity<ParameterEntity>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasMany(x => x.Lots)
            .WithMany(x => x.Parameters);

            entity.HasMany(x => x.Values)
            .WithOne(x => x.Parameter)
            .HasForeignKey(x => x.ParameterId);

            entity.Property(x => x.Name)
            .IsRequired();
        });

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

            entity.HasOne(x => x.AiEstimatedValue)
            .WithOne(x => x.Lot)
            .HasForeignKey<AiEstimatedValueEntity>(x => x.LotId);

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

        modelBuilder.Entity<ExchangeOfferEntity>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.TargetLot)
            .WithMany(x => x.TargetOffers)
            .HasForeignKey(x => x.TargetLotId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.OfferLot)
            .WithMany(x => x.TargetedOffers)
            .HasForeignKey(x => x.OfferLotId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Sender)
            .WithMany(x => x.SentOffers)
            .HasForeignKey(x => x.SenderId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Receiver)
            .WithMany(x => x.ReceivedOffers)
            .HasForeignKey(x => x.ReceiverId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

            entity.Property(x => x.Status)
            .IsRequired();

            entity.Property(x => x.CreatedAt)
            .HasConversion(x => x.ToUniversalTime(), x => x)
            .IsRequired();

            entity.Property(x => x.UpdatedAt)
            .HasConversion(x => x.ToUniversalTime(), x => x)
            .IsRequired();
        });

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
