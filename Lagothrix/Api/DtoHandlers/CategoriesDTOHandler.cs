using Api.DtoHandlers.Interfaces;
using Api.DTOs.Category.Response;
using Api.DTOs.Lot.Response;
using Logic.Services.Interfaces;

namespace Api.DtoHandlers;

public class CategoriesDTOHandler : ICategoriesDTOHandler
{
    private readonly ICategoriesService _categoriesService;

    public CategoriesDTOHandler(ICategoriesService service)
    {
        _categoriesService = service;
    }

    public async Task<CategoriesListResponse> GetAllAsync(CancellationToken cancellationToken)
    {
        var categories = (await _categoriesService
            .GetAllAsync(cancellationToken))
            .Select(category => new CategoryResponse(category.Id, category.Name));
        return new([.. categories]);
    }

    public async Task<LotsListResponse> GetLotsByCategoryIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var lots = await _categoriesService.GetLotsByCategoryIdAsync(id, cancellationToken);
        IEnumerable<LotResponse> lotsListResponse = lots
            .Select(lot => new LotResponse(
                lot.Id,
                lot.Title,
                lot.Description,
                lot.Type,
                lot.EstimatedValue,
                lot.DesiredExchange,
                lot.Status,
                lot.CreatedAt,
                lot.PublishedAt,
                lot.AiEstimatedValue ?? 0,
                lot.Owner.Id,
                lot.Category.Id
            ));
        return new([.. lotsListResponse]);
    }
}
