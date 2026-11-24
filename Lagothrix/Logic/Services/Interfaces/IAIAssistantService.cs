namespace Logic.Services.Interfaces;

public interface IAiAssistantService
{
    public Task<decimal> GetPriceRecommendationAsync(EstimationRequest lot, CancellationToken cancellationToken);
}