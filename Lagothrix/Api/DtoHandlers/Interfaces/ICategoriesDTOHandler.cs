using Api.DTOs.Category.Response;
using Api.DTOs.Lot.Response;

namespace Api.DtoHandlers.Interfaces;

public interface ICategoriesDTOHandler
{
    public Task<CategoriesListResponse> GetAllAsync(CancellationToken cancellationToken);
    public Task<LotsListResponse> GetLotsByCategoryIdAsync(Guid id, CancellationToken cancellationToken);
}