using Dal.Entities.Lot;

namespace Dal.Entities;
public class AiEstimatedValueEntity
{
    public Guid Id { get; set; }
    public decimal EstimatedValue { get; set; }
    public DateTime CreatedAt { get; set; }

    public LotEntity Lot { get; set; }
    public Guid LotId { get; set; }
}
