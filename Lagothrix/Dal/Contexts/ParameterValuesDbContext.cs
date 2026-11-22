using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Dal.Contexts;
public class ParameterValuesDbContext : DbContext
{
    public ParameterValuesDbContext(DbContextOptions<ParameterValuesDbContext> options) : base(options) { }
    public DbSet<ParameterValueEntity> ParameterValueEntity => Set<ParameterValueEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
    }

}
