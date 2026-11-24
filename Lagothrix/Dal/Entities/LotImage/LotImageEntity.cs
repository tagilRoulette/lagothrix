using Dal.Entities.Lot;

namespace Dal.Entities.LotImage;
public class LotImageEntity
{
    public LotImageEntity(
        Guid id,
        string url,
        int sortOrder,
        LotEntity lot,
        Guid lotId)
    {
        Id = id;
        Url = url;
        SortOrder = sortOrder;
        Lot = lot;
        LotId = lotId;
    }

    public Guid Id { get; set; }
    public string Url { get; set; }
    public int SortOrder { get; set; }

    public LotEntity Lot { get; set; }
    public Guid LotId { get; set; }
}
