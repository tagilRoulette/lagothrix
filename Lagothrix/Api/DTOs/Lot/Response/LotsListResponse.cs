using Api.DTOs.Lot.Response;

namespace Api;

public record LotsListResponse
{
    public IReadOnlyCollection<LotResponse> Lots { get; set; }

    public LotsListResponse(IReadOnlyCollection<LotResponse> lots)
    {
        Lots = lots;
    }
}