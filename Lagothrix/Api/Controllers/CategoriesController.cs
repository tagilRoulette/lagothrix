using Api.DTOs.Category.Response;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/api/categories")]
public class CategoriesController : Controller
{
    private ICategoriesDTOHandler _dtoHandler;

    public CategoriesController(ICategoriesDTOHandler dtoHandler)
    {
        _dtoHandler = dtoHandler;
    }

    [HttpGet]
    public async Task<ActionResult<CategoriesListResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await _dtoHandler.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [Route("{categoryId:guid}/lots")]
    public async Task<ActionResult<LotsListResponse>> GetLotsByCategoryIdAsync(
        [FromRoute] Guid categoryId,
        CancellationToken cancellationToken
        )
    {
        var result = await _dtoHandler.GetLotsByCategoryIdAsync(categoryId, cancellationToken);
        return Ok(result);
    }
}
