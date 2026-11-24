using System.Linq.Expressions;
using System.Reflection;
using Api.DTOs.Lot.Request;
using Dal;
using Dal.Entities;
using Dal.Entities.Lot;
using Logic;
using Logic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/api/lots")]
public class LotsController : Controller
{
    private readonly NpgsqlDbContext _context;
    private readonly IAiAssistantService _assistantService;

    public LotsController(
        NpgsqlDbContext context,
        IAiAssistantService assistantService)
    {
        _context = context;
        _assistantService = assistantService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromBody] GetAllLotsRequest request,
        CancellationToken cancellationToken
        )
    {
        IEnumerable<LotEntity> lots = GetLotsWithAppliedRequestParameters(request, _context);

        IReadOnlyCollection<LotEntity> lotsCollection = [.. lots];
        if (lotsCollection.Count == 0) return NoContent();
        return Ok(lots);
    }

    private static IEnumerable<LotEntity> GetLotsWithAppliedRequestParameters(GetAllLotsRequest request, NpgsqlDbContext context)
    {
        IEnumerable<LotEntity> lots = context.LotEntity;
        if (request.CategoryId != null)
            lots = lots.Where(lot => lot.CategoryId == request.CategoryId);
        if (request.LotType != null)
            lots = lots.Where(lot => lot.Type == request.LotType);
        if (request.SearchQuery != null)
            lots = lots.Where(lot => lot.Title.Contains(request.SearchQuery));
        if (request.EstimatedValueFork != null)
            lots = lots.Where(lot => lot.Title.Contains(request.SearchQuery));
        MethodCallExpression orderByCall = GetOrderByExpression(request, lots);
        lots = (IEnumerable<LotEntity>)
            Expression.Lambda(orderByCall).Compile().DynamicInvoke();
        return lots;
    }

    private static MethodCallExpression GetOrderByExpression(GetAllLotsRequest request, IEnumerable<LotEntity> lots)
    {
        var lotType = typeof(LotEntity);
        var prop = lotType.GetProperty(request.SortByFieldName, BindingFlags.Instance | BindingFlags.Public)
                   ?? lotType.GetProperty("PublishedAt", BindingFlags.Instance | BindingFlags.Public);
        var lotVariable = Expression.Variable(lotType);
        var propertyAccessor = Expression.Property(lotVariable, request.SortByFieldName);
        var sortablePropSelector = Expression.Lambda(propertyAccessor, lotVariable);
        var orderBy = typeof(Enumerable)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(m => m.Name == nameof(Enumerable.OrderBy) && m.GetParameters().Length == 2)
            .MakeGenericMethod(lotType, prop.PropertyType);
        var call = Expression.Call(
            orderBy,
            Expression.Constant(lots, typeof(IEnumerable<LotEntity>)),
            Expression.Quote(sortablePropSelector));
        return call;
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
        )
    {
        LotEntity? lot = await _context.LotEntity.FindAsync([id], cancellationToken: cancellationToken);
        if (lot == null) return NotFound();
        return Ok(lot);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLotAsync(
        [FromBody] CreateLotRequest request,
        CancellationToken cancellationToken
        )
    {
        LotEntity lot = new()
        {
            Title = request.Title,
            Description = request.Description,
            CategoryId = request.CategoryId,
            Type = request.LotType,
            EstimatedValue = request.EstimatedValue,
            DesiredExchange = request.DesiredExchange,

            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Status = LotStatus.Draft,
        };

        var categoryName = (await _context.CategoryEntity.FindAsync([request.CategoryId], cancellationToken)).Name;
        var aiEstimatedValue = await _assistantService.GetPriceRecommendationAsync(
            new EstimationRequest(
                request.Title,
                request.Description,
                request.LotType.ToString(format: null),
                categoryName
            ), cancellationToken);
        AiEstimatedValueEntity estimatedValueEntity = new()
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            EstimatedValue = aiEstimatedValue,
            LotId = lot.Id,
            Lot = lot
        };
        _context.AiEstimatesEntity.Add(estimatedValueEntity);
        lot.AiEstimatedValue = estimatedValueEntity;

        return Ok(lot);
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> ChangeLotByIdAsync(
        [FromRoute] Guid id,
        [FromBody] CreateLotRequest request,
        CancellationToken cancellationToken
        )
    {
        if ((await _context.CategoryEntity.FindAsync([request.CategoryId], cancellationToken)) is not CategoryEntity category)
            return NotFound(request.CategoryId);
        string categoryName = category.Name;
        LotEntity newLot = new()
        {
            Title = request.Title,
            Description = request.Description,
            CategoryId = request.CategoryId,
            Type = request.LotType,
            EstimatedValue = request.EstimatedValue,
            DesiredExchange = request.DesiredExchange,

            Id = id,
            CreatedAt = DateTime.UtcNow,
            Status = LotStatus.Active,
        };
        var lot = await _context.LotEntity.FindAsync([id], cancellationToken);
        if (lot == null) return NotFound(id);

        var newAiEstimatedValue = await _assistantService.GetPriceRecommendationAsync(
            new EstimationRequest(
                request.Title,
                request.Description,
                request.LotType.ToString(format: null),
                categoryName
            ), cancellationToken);

        lot.AiEstimatedValue.EstimatedValue = newAiEstimatedValue;
        _context.AiEstimatesEntity.Update(lot.AiEstimatedValue);
        newLot.AiEstimatedValue = lot.AiEstimatedValue;
        _context.LotEntity.Update(newLot);

        return Ok(newLot);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteLotByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
        )
    {
        var lot = await _context.LotEntity.FindAsync([id], cancellationToken);
        if (lot == null) return NotFound();
        _context.LotEntity.Remove(lot);
        return Ok(lot);
    }

    [HttpPost]
    [Route("{id:guid}/publish")]
    public async Task<IActionResult> PublishLotAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
        )
    {
        var lot = await _context.LotEntity.FindAsync([id], cancellationToken);
        if (lot == null) return NotFound(id);
        lot.Status = LotStatus.Active;
        _context.LotEntity.Update(lot);
        return Ok(lot);
    }
}
