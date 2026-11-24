using Dal.Entities;
using Dal.Entities.Lot;

namespace Logic.Models;
public record LotModel
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
    public decimal? AiEstimatedValue { get; }
    public UserEntity Owner { get; set; }
    public CategoryEntity Category { get; set; }
}
