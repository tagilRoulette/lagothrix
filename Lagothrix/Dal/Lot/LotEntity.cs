using Dal.ExchangeOffer;
using Dal.LotImage;

namespace Dal.Lot;
public class LotEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public LotType Type {  get; set; }
    public decimal? EstimatedValue {  get; set; }
    public string? DesiredExchange { get; set; }
    public LotStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime PublishedAt { get; set; } = DateTime.MinValue; // if was never published

    public UserEntity User { get; set; }
    public Guid UserId { get; set; }

    public CategoryEntity Category { get; set; }
    public Guid CategoryId { get; set; }

    public AiEstimatedWorthEntity AiEstimatedWorth { get; set; }
    public List<ParameterEntity> Parameters { get; set; } = [];
    public List<LotImageEntity> Images { get; set; } = [];
    public List<ExchangeOfferEntity> TargetOffers { get; set; } = [];
    public List<ExchangeOfferEntity> TargetedOffers { get; set; } = [];
}
