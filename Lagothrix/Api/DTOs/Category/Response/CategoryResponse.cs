namespace Api.DTOs.Category.Response;

public record CategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public CategoryResponse(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}
