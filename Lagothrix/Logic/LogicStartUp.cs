using Logic.Services;
using Logic.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Logic;
public static class LogicStartUp
{
    public static void AddLogic(this IServiceCollection services)
    {
        services.AddScoped<ICategoriesService, CategoriesService>();
        services.AddScoped<IAiAssistantService, PlaceholderAiAssistant>();
        services.AddScoped<AiPricePredictorService>();
    }
}
