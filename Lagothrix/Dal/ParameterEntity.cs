using Dal.Lot;

namespace Dal;
public class ParameterEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public List<ParameterValueEntity> Values { get; set; } = [];
    public List<LotEntity> Lots { get; set; } = [];
}
