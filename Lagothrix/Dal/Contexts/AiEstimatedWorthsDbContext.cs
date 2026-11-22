using Microsoft.EntityFrameworkCore;

namespace Dal.Contexts;
public class AiEstimatedWorthsDbContext : DbContext
{
    public AiEstimatedWorthsDbContext(DbContextOptions<AiEstimatedWorthsDbContext> options) : base(options) { }
    public DbSet<AiEstimatedWorthEntity> AiEstimatesEntity => Set<AiEstimatedWorthEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AiEstimatedWorthEntity>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.Lot)
            .WithOne(x => x.AiEstimatedWorth)
            .HasForeignKey<AiEstimatedWorthEntity>(x => x.LotId);
        });
    }
}
