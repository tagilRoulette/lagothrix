using Api.DTOs.Category.Response;

namespace Api;

public interface ICategoriesDTOHandler
{
    public Task<CategoriesListResponse> GetAllAsync(CancellationToken cancellationToken);
    public Task<LotsListResponse> GetLotsByCategoryIdAsync(Guid id, CancellationToken cancellationToken);
}