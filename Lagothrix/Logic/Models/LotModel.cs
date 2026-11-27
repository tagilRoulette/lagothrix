using Dal.Entities;
using Dal.Entities.Lot;

namespace Logic.Models;
public record LotModel
(
    Guid Id,
    string Title,
    string? Description,
    LotType Type,
    decimal? EstimatedValue,
    string? DesiredExchange,
    LotStatus Status,
    DateTime CreatedAt,
    DateTime PublishedAt,
    decimal? AiEstimatedValue,
    UserEntity Owner,
    CategoryEntity Category
);
