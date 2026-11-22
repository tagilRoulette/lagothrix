using Microsoft.EntityFrameworkCore;

namespace Dal.Contexts;
public class CategoriesDbContext : DbContext
{
    public CategoriesDbContext(DbContextOptions<CategoriesDbContext> options) : base(options) { }
    public DbSet<CategoryEntity> LotImageEntity => Set<CategoryEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
    }
}
