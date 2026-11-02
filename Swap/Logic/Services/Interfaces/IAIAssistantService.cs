namespace Logic.Services.Interfaces;

public interface IAIAssistantService
{
    public Task<PriceFork> GetPriceRecommendation(LotModel lot, CancellationToken cancellationToken);
}