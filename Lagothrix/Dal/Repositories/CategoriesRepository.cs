using Dal.Entities;
using Dal.Entities.Lot;
using Dal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories;
public class CategoriesRepository : ICategoriesRepository
{
    private readonly NpgsqlDbContext _context;

    public CategoriesRepository(NpgsqlDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<CategoryEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Categories.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<LotEntity>> GetLotsByCategoryIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Lots.Where(x => x.CategoryId == id).ToListAsync(cancellationToken);
    }

}
