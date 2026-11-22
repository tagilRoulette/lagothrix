using Dal.Lot;

namespace Dal.LotImage;
public class LotImageEntity
{
    public Guid Id { get; set; }
    public string Url { get; set; }
    public SortOrder SortOrder { get; set; }

    public LotEntity Lot { get; set; }
    public Guid LotId { get; set; }
}
