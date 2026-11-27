namespace Api.DTOs.Lot.Response;

public record LotsListResponse
{
    public IReadOnlyCollection<LotResponse> Lots { get; set; }

    public LotsListResponse(IReadOnlyCollection<LotResponse> lots)
    {
        Lots = lots;
    }
}