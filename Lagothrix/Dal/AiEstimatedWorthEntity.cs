using Dal.Lot;

namespace Dal;
public class AiEstimatedWorthEntity
{
    public Guid Id { get; set; }
    public decimal EstimatedValue { get; set; }
    public DateTime CreatedAt { get; set; }

    public LotEntity Lot { get; set; }
    public Guid LotId { get; set; }
}
