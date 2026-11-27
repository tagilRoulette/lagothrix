using Dal.Entities.ExchangeOffer;
using Dal.Entities.Lot;
using Microsoft.AspNetCore.Identity;

namespace Dal.Entities;

public class UserEntity : IdentityUser<Guid>
{
    public DateTime CreatedAt { get; set; }

    public ICollection<LotEntity> Lots { get; set; } = [];
    public ICollection<ExchangeOfferEntity> SentOffers { get; set; } = [];
    public ICollection<ExchangeOfferEntity> ReceivedOffers { get; set; } = [];
}
