using Microsoft.EntityFrameworkCore;

namespace Dal.Contexts;

public class ParametersDbContext : DbContext
{
    public ParametersDbContext(DbContextOptions<ParametersDbContext> options) : base(options) { }
    public DbSet<ParameterEntity> ParameterEntity => Set<ParameterEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
    }
}