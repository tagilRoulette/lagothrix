using Dal.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dal;
public static class DalStartUp
{
    public static void AddDal(this IServiceCollection services, string connectionString)
    {
        services.AddDbContextFactory<UsersDbContext>(options =>
            options.UseNpgsql(connectionString));
        services.AddDbContextFactory<CategoriesDbContext>(options =>
            options.UseNpgsql(connectionString));
        services.AddDbContextFactory<ExchangeOffersDbContext>(options =>
            options.UseNpgsql(connectionString));
        services.AddDbContextFactory<LotsDbContext>(options =>
            options.UseNpgsql(connectionString));
        services.AddDbContextFactory<LotImagesDbContext>(options =>
            options.UseNpgsql(connectionString));
        services.AddDbContextFactory<AiEstimatedWorthsDbContext>(options =>
            options.UseNpgsql(connectionString));
        services.AddDbContextFactory<ParametersDbContext>(options =>
            options.UseNpgsql(connectionString));
        services.AddDbContextFactory<ParameterValuesDbContext>(options =>
            options.UseNpgsql(connectionString));
    }
}
