using Microsoft.EntityFrameworkCore;

namespace Dal.Contexts;
public class UsersDbContext : DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options) { }
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
            .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(x => x.SentOffers)
            .WithOne(x => x.Sender)
            .HasForeignKey(x => x.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

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

        base.OnModelCreating(modelBuilder);
    }
}
