using Dal.Lot;

namespace Dal;
public class CategoryEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public List<LotEntity> Lots { get; set; }
}
