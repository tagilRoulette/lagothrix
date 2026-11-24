using System;
using System.Collections.Generic;
using System.Text;
using Dal.Entities;
using Dal.Entities.Lot;

namespace Dal.Repositories;
public class CategoryRepository
{
    private readonly NpgsqlDbContext _context;

    public CategoryRepository(NpgsqlDbContext context)
    {
        _context = context;
    }

    public Task<ICollection<CategoryEntity>> GetAllAsync(CancellationToken cancellationToken);
    public Task<ICollection<LotEntity>> GetLotsByCategoryIdAsync(Guid id, CancellationToken cancellationToken);

}
