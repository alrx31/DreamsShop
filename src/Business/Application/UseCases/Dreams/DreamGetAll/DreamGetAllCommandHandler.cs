using Application.DTO;
using Application.UseCases.Base;
using Domain.Entity;
using Domain.IService;
using Domain.Model;

namespace Application.UseCases.Dreams.DreamGetAll;

public class DreamGetAllCommandHandler(
    IFileStorageService fileStorageService,
    ICacheService<DreamCacheKey, List<DreamResponseDto>> cacheService
    ) : BaseRequestHandler<DreamGetAllCommand,List<DreamResponseDto>>
{
    private bool useCache = false;
    public override async Task<List<DreamResponseDto>> Handle(DreamGetAllCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = new DreamCacheKey
        {
            StartIndex = request.StartIndex,
            Count = request.Count
        };

        if (request.StartIndex == DreamCacheKey.DefaultStartIndex && request.Count == DreamCacheKey.DefaultCount)
        {
            useCache = true;

            var cachedDreams = await cacheService.GetAsync(cacheKey);
            if (cachedDreams is not null) return cachedDreams;
        }

        var dreams = await UnitOfWork.DreamRepository
                .GetAsync<Dream>(
                    skip: request.StartIndex,
                    take: request.Count,
                    cancellationToken: cancellationToken) ?? new List<Dream>();

        if (dreams.Count == 0) return [];

        var imageTasks = dreams
            .Select(d => fileStorageService.DownloadFileAsync(d.ImageFileName, cancellationToken));
        var imageResults = await Task.WhenAll(imageTasks);

        var imageBytesList = new List<byte[]>(dreams.Count);
        var imageContentTypes = new List<string?>(dreams.Count);
        foreach (var imageResult in imageResults)
        {
            byte[] imageBytes = Array.Empty<byte>();
            if (imageResult?.Content is not null)
            {
                using var stream = new MemoryStream();
                await imageResult.Content.CopyToAsync(stream, cancellationToken);
                imageBytes = stream.ToArray();
            }

            imageBytesList.Add(imageBytes);
            imageContentTypes.Add(imageResult?.ContentType);
        }

        var dreamIds = dreams.Select(d => d.DreamId).ToList();
        var categories = await UnitOfWork.CategoryRepository.GetAsync<Domain.Entity.Category>(cancellationToken: cancellationToken)
            ?? new List<Domain.Entity.Category>();

        var dreamCategoryMap = new Dictionary<Guid, List<CategoryResponseDto>>();
        foreach (var dreamId in dreamIds)
        {
            var dreamCategoriesQuery = await UnitOfWork.DreamCategoryRepository.GetCategoriesByDreamIdAsync(dreamId, cancellationToken);
            var dreamCategories = dreamCategoriesQuery?.ToList() ?? new List<Domain.Entity.DreamCategory>();
            var categoryIds = dreamCategories.Select(dc => dc.CategoryId).ToList();
    
            var matchedCategories = categories
                .Where(c => categoryIds.Contains(c.CategoryId))
                .Select(c => new CategoryResponseDto
                {
                    CategoryId = c.CategoryId,
                    Title = c.Title,
                    Description = c.Description
                });

            dreamCategoryMap[dreamId] = matchedCategories.ToList();
        }

        var result = dreams.ToList().Select((dream, index) => new DreamResponseDto
        {
            Id = dream.DreamId,
            Title = dream.Title,
            Description = dream.Description,
            ProducerId = dream.ProducerId,
            Rating = dream.Rating,

            Categories = dreamCategoryMap[dream.DreamId],

            ImageBase64 = Convert.ToBase64String(imageBytesList[index]),
            ImageContentType = imageContentTypes[index]
        }).ToList();

        if (useCache)
        {
            await cacheService.SetAsync(cacheKey, result);
            useCache = false;
        }

        return result;
    }
}