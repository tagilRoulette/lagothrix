namespace Api.DTOs.Category.Response;

public record CategoriesListResponse
{
    public IReadOnlyCollection<CategoryResponse> Categories { get; set; }

    public CategoriesListResponse(IReadOnlyCollection<CategoryResponse> categories)
    {
        Categories = categories;
    }
}
