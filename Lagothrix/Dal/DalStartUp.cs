using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dal;
public static class DalStartUp
{
    public static void AddDal(this IServiceCollection services, string connectionString)
    {
        services.AddDbContextFactory<NpgsqlDbContext>(options =>
            options.UseNpgsql(connectionString));
    }
}
