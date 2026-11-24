using Dal.Entities.Lot;
using Logic;

namespace Api.DTOs.Lot.Request;

public record GetAllLotsRequest
{
    public Guid? CategoryId { get; set; }
    public LotType? LotType { get; set; }
    public string? SearchQuery { get; set; }
    public PriceFork? EstimatedValueFork { get; set; }
    public bool IsSortAscending { get; set; }
    public string SortByFieldName { get; set; }
}
