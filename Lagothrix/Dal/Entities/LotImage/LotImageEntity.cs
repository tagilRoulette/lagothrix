using Dal.Entities.Lot;

namespace Dal.Entities.LotImage;
public class LotImageEntity
{
    public Guid Id { get; set; }
    public string Url { get; set; }
    public int SortOrder { get; set; }

    public LotEntity Lot { get; set; }
    public Guid LotId { get; set; }
}
