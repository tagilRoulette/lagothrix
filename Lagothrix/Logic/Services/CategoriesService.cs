using Dal.Repositories.Interfaces;
using Logic.Models;
using Logic.Services.Interfaces;

namespace Logic.Services;
public class CategoriesService : ICategoriesService
{
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly IAiAssistantService _pricePredictorService;

    public CategoriesService(
        ICategoriesRepository categoriesRepository,
        IAiAssistantService pricePredictorService)
    {
        _categoriesRepository = categoriesRepository;
        _pricePredictorService = pricePredictorService;
    }

    public async Task<ICollection<CategoryModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        var categoryEntities = await _categoriesRepository.GetAllAsync(cancellationToken);
        return [.. categoryEntities.Select(c => new CategoryModel(c.Id, c.Name))];
    }

    public async Task<ICollection<LotModel>> GetLotsByCategoryIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var lotEntities = await _categoriesRepository.GetLotsByCategoryIdAsync(id, cancellationToken);
        var lotModelsTask = lotEntities
            .Where(c => c.Id == id)
            .Select(async c => new LotModel(
                c.Id, c.Title, c.Description, c.Type, c.EstimatedValue,
                c.DesiredExchange, c.Status, c.CreatedAt, c.PublishedAt,
                await _pricePredictorService.GetPriceRecommendationAsync(
                    new(c.Title, c.Description, c.Type.ToString(), c.Category.Name), cancellationToken
                ), c.User, c.Category)
            );

        List<LotModel> result = new();
        foreach (var task in lotModelsTask)
        {
            result.Add(await task);
        }
        return result;
    }
}
