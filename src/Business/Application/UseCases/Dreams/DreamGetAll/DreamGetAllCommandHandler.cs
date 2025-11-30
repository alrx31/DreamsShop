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
                    cancellationToken: cancellationToken);

        if (!dreams.Any()) return [];

        var imageFileNames = dreams.Select(d => d.ImageFileName).ToList();

        var imageTasks = imageFileNames.Select(f => fileStorageService.DownloadFileAsync(f, cancellationToken));
        var imageResults = await Task.WhenAll(imageTasks);

        var imageBytesList = new List<byte[]>();
        foreach (var imageResult in imageResults)
        {
            using var stream = new MemoryStream();
            await imageResult.Content!.CopyToAsync(stream, cancellationToken);
            imageBytesList.Add(stream.ToArray());
        }

        var dreamIds = dreams.Select(d => d.DreamId).ToList();

        var dreamCategoryMap = new Dictionary<Guid, List<CategoryResponseDto>>();
        foreach (var dreamId in dreamIds)
        {
            var dreamCategories = await UnitOfWork.DreamCategoryRepository.GetCategoriesByDreamIdAsync(dreamId, cancellationToken);
            var categoryIds = dreamCategories.Select(dc => dc.CategoryId).ToList();
    
            var categories = await UnitOfWork.CategoryRepository.GetAsync<Domain.Entity.Category>(cancellationToken:cancellationToken);
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
            ImageContentType = imageResults[index].ContentType
        }).ToList();

        if (useCache)
        {
            await cacheService.SetAsync(cacheKey, result);
            useCache = false;
        }

        return result;
    }
}