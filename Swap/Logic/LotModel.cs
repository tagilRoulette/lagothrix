namespace Logic;
public class LotModel
{
    public LotModel(string name, string region, string? description)
    {
        Name = name;
        Region = region;
        Description = description;
    }

    public string Name { get; set; }
    public string Region { get; set; }
    public string? Description { get; set; }

    public override string ToString()
    {
        return $"Name: {Name}, Region: {Region}, Description: {Description}";
    }
}
