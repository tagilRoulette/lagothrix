using Dal.Entities.Lot;

namespace Dal.Entities;
public class ParameterEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public ICollection<ParameterValueEntity> Values { get; set; } = [];
    public ICollection<LotEntity> Lots { get; set; } = [];
}
