using Logic.Services.Interfaces;

namespace Logic.Services;
public class PlaceholderAiAssistant : IAiAssistantService
{
    public Task<decimal> GetPriceRecommendationAsync(EstimationRequest lot, CancellationToken cancellationToken)
    {
        return Task.Run(() => new decimal(0));
    }
}
