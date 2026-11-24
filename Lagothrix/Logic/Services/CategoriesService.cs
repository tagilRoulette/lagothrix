using System;
using System.Collections.Generic;
using System.Text;
using Logic.Models;
using Logic.Services.Interfaces;

namespace Logic.Services;
public class CategoriesService : ICategoriesService
{
    public async Task<ICollection<CategoryModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<LotModel>> GetLotsByCategoryIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
