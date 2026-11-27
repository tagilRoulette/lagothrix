using Dal.Entities;
using Dal.Entities.Lot;

namespace Dal.Repositories.Interfaces;
public interface ICategoriesRepository
{
    public Task<IReadOnlyCollection<CategoryEntity>> GetAllAsync(CancellationToken cancellationToken);
    public Task<IReadOnlyCollection<LotEntity>> GetLotsByCategoryIdAsync(Guid id, CancellationToken cancellationToken);
}
