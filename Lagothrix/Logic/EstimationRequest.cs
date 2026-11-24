namespace Logic;

public class EstimationRequest
{
    public EstimationRequest(
        string title,
        string? description,
        string lotType,
        string category)
    {
        Title = title;
        Description = description;
        LotType = lotType;
        Category = category;
    }

    public string Title { get; set; }
    public string? Description { get; set; }
    public string LotType { get; set; }
    public string Category { get; set; }
}
