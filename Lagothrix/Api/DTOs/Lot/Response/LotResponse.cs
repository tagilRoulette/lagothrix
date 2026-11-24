using Dal.Entities.Lot;

namespace Api.DTOs.Lot.Response;

public record LotResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public LotType Type { get; set; }
    public decimal? EstimatedValue { get; set; }
    public string? DesiredExchange { get; set; }
    public LotStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime PublishedAt { get; set; }
    public decimal AiEstimatedWorth { get; set; }

    public Guid OwnerId { get; set; }
    public Guid CategoryId { get; set; }

    public LotResponse(
        Guid id,
        string title,
        string? description,
        LotType type,
        decimal? estimatedValue,
        string? desiredExchange,
        LotStatus status,
        DateTime createdAt,
        DateTime publishedAt,
        decimal aiEstimatedWorth,
        Guid ownerId,
        Guid categoryId)
    {
        Id = id;
        Title = title;
        Description = description;
        Type = type;
        EstimatedValue = estimatedValue;
        DesiredExchange = desiredExchange;
        Status = status;
        CreatedAt = createdAt;
        PublishedAt = publishedAt;
        AiEstimatedWorth = aiEstimatedWorth;
        OwnerId = ownerId;
        CategoryId = categoryId;
    }
}