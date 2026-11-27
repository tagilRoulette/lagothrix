using Api.DtoHandlers.Interfaces;

namespace Api.DtoHandlers;

public static class DtoHandlersStartUp
{
    public static void AddDtoHandlers(this IServiceCollection services)
    {
        services.AddScoped<ICategoriesDTOHandler, CategoriesDTOHandler>();
    }
}
