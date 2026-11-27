using Logic.Models;

namespace Logic.Services.Interfaces;
public interface ICategoriesService
{
    public Task<ICollection<CategoryModel>> GetAllAsync(CancellationToken cancellationToken);
    public Task<ICollection<LotModel>> GetLotsByCategoryIdAsync(Guid id, CancellationToken cancellationToken);
}
