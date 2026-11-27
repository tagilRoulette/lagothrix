using Dal.Entities;
using Dal.Repositories;
using Dal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dal;
public static class DalStartUp
{
    public static void AddDal(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<ICategoriesRepository, CategoriesRepository>();

        services.AddDbContextFactory<NpgsqlDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<NpgsqlDbContext>(sp =>
            sp.GetRequiredService<IDbContextFactory<NpgsqlDbContext>>()
              .CreateDbContext());

        services.AddDefaultIdentity<UserEntity>()
            .AddEntityFrameworkStores<NpgsqlDbContext>();
    }
}
