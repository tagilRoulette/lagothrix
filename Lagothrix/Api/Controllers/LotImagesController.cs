using System.Runtime.CompilerServices;
using Dal;
using Dal.Entities.Lot;
using Dal.Entities.LotImage;
using Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers;

[ApiController]
public class LotImagesController : Controller
{
    private readonly NpgsqlDbContext _context;
    private readonly IOptions<UploadConfig> _uploadOptions;

    public LotImagesController(
        NpgsqlDbContext context,
        IOptions<UploadConfig> uploadOptions)
    {
        _context = context;
        _uploadOptions = uploadOptions;
    }

    [HttpGet]
    [Route("/api/lot/{lotId:guid}/images")]
    public async Task<IActionResult> GetAllByLotIdAsync(
        [FromRoute] Guid lotId
        )
    {
        var paths = await Task.Run(() => _context.
            LotImageEntity
            .Where(x => x.LotId == lotId)
            .Select(x => x.Url));
        if (!paths.Any()) return NotFound();
        IReadOnlyCollection<string> pathsCollection = [.. paths];
        return Ok(pathsCollection);
    }

    [HttpPost]
    [Route("/api/lot/{lotId:guid}/images")]
    public async Task<IActionResult> AddByLotIdAsync(
        [FromRoute] Guid lotId,
        [FromForm] IFormFileCollection images,
        CancellationToken cancellationToken
        )
    {
        var findLotTask = _context.LotEntity.FindAsync([lotId], cancellationToken);
        var imagesUploadInfo = UploadFilesAsync(images, cancellationToken);
        var imageEntitesCount = _context.LotImageEntity.Count(img => img.LotId == lotId);
        var lot = await findLotTask;

        var imageEntities = imagesUploadInfo
            .Select((path, i) => new LotImageEntity(
                Guid.NewGuid(),
                path,
                imageEntitesCount + i,
                lot!,
                lotId
            ));
        await _context.LotImageEntity.AddRangeAsync(
            imageEntities.ToBlockingEnumerable(cancellationToken),
            cancellationToken);
        return NoContent();
    }

    [HttpPut]
    [Route("/api/images/{id:guid}")]
    public async Task<IActionResult> ChangeImageAsync(
        [FromRoute] Guid id,
        [FromForm] IFormFile image,
        CancellationToken cancellationToken
        )
    {
        var imageEntity = await _context.LotImageEntity.FindAsync([id], cancellationToken);
        if (imageEntity == null) return NotFound();
        var path = await UploadFilesAsync([image], cancellationToken).SingleAsync();
        imageEntity.Url = path;
        _context.LotImageEntity.Update(imageEntity);
        return Ok(imageEntity);
    }

    [HttpDelete]
    [Route("/api/images/{id:guid}")]
    public async Task<IActionResult> DeleteImageAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
        )
    {
        var image = await _context.LotImageEntity.FindAsync([id], cancellationToken);
        if (image == null) return NotFound();
        _context.LotImageEntity.Remove(image);
        return Ok(image);
    }

    [HttpDelete]
    [Route("/api/lot/{lotId:guid}/images")]
    public async Task<IActionResult> DeleteAllImagesFromLotAsync(
        [FromRoute] Guid lotId,
        CancellationToken cancellationToken
        )
    {
        _context.LotImageEntity.RemoveRange(
            _context.LotImageEntity.Where(x => x.LotId == lotId)
            );
        return NoContent();
    }

    private async IAsyncEnumerable<string> UploadFilesAsync(IEnumerable<IFormFile> files, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        List<string> uploadedFilePaths = new();
        Directory.CreateDirectory(_uploadOptions.Value.FileSavePath);
        foreach (var file in ValidateImages(files, _uploadOptions.Value.SupportedFormats))
        {
            var fileId = Guid.NewGuid();
            string fullPath = $"{_uploadOptions.Value.FileSavePath}/" +
                $"{fileId}";

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream, cancellationToken);
            }

            yield return fullPath;
        }
    }

    private static IEnumerable<IFormFile> ValidateImages(IEnumerable<IFormFile> files, ICollection<string> supportedFormats)
    {
        supportedFormats = supportedFormats
            .Select(x => x.TrimStart('.'))
            .ToHashSet();
        return files
            .Select(file => (file, file.FileName.Split('.')))
            .Where(imageInfo => imageInfo.Item2.Length > 1 
                && supportedFormats.Contains(imageInfo.Item2[^1]))
            .Select(fileInfo => fileInfo.file);
    }
}
