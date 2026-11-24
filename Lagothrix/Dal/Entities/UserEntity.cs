using Dal.Entities.ExchangeOffer;
using Dal.Entities.Lot;

namespace Dal.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<LotEntity> Lots { get; set; } = [];
    public ICollection<ExchangeOfferEntity> SentOffers { get; set; } = [];
    public ICollection<ExchangeOfferEntity> ReceivedOffers { get; set; } = [];
}
