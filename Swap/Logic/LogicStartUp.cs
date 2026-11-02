using Logic.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Logic;
public static class LogicStartUp
{
    public static void AddLogic(this IServiceCollection services)
    {
        services.AddScoped<AiPricePredictorService>();
    }
}
