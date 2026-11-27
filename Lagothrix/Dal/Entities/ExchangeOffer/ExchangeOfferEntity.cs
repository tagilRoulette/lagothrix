using Dal.Entities.Lot;

namespace Dal.Entities.ExchangeOffer;
public class ExchangeOfferEntity
{
    public Guid Id { get; set; }
    public OfferStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public LotEntity OfferLot { get; set; }
    public Guid OfferLotId { get; set; }

    public LotEntity TargetLot { get; set; }
    public Guid TargetLotId { get; set; }


    public UserEntity Sender { get; set; }
    public Guid SenderId { get; set; }

    public UserEntity Receiver { get; set; }
    public Guid ReceiverId { get; set; }
}
