using Dal.Entities.Lot;

namespace Api.DTOs.Lot.Request;

public class ChangeLotRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Guid? CategoryId { get; set; }
    public LotType? LotType { get; set; }
    public decimal? EstimatedValue { get; set; }
    public string? DesiredExchange { get; set; }
}
