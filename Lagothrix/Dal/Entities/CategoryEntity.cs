using Dal.Entities.Lot;

namespace Dal.Entities;
public class CategoryEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public ICollection<LotEntity> Lots { get; set; }
}
