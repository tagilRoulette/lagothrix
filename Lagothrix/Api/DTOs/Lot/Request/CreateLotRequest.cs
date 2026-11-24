using Dal.Entities.Lot;

namespace Api.DTOs.Lot.Request;

public record CreateLotRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid CategoryId { get; set; }
    public LotType LotType { get; set; }
    public decimal? EstimatedValue { get; set; }
    public string? DesiredExchange { get; set; }
}
