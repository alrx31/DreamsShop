using Application.DTO;
using Application.Exceptions;
using Application.UseCases.Base;
using Domain.Entity;
using Domain.IService;

namespace Application.UseCases.Dreams.DreamsGetOne;

public class DreamGetOneCommandHandler(
        IFileStorageService fileStorageService,
        ICacheService<string, DreamResponseDto> cacheService
    ) : BaseRequestHandler<DreamGetOneCommand, DreamResponseDto?>
{
    public override async Task<DreamResponseDto?> Handle(DreamGetOneCommand request, CancellationToken cancellationToken)
    {
        var cachedDream = await cacheService.GetAsync(request.DreamId.ToString() + nameof(Dream));
        if (cachedDream is not null) return cachedDream;

        var dream = await UnitOfWork.DreamRepository.GetAsync([request.DreamId], cancellationToken);

        if (dream is null) throw new NotFoundException("Dream not found.");
        
        var dreamImg = await fileStorageService.DownloadFileAsync(dream.ImageFileName, cancellationToken);
        using var stream = new MemoryStream();
        await dreamImg.Content!.CopyToAsync(stream, cancellationToken);
        var imageBytes = stream.ToArray();
        
        var dreamCategoriesQuery = await UnitOfWork.DreamCategoryRepository.GetCategoriesByDreamIdAsync(dream.DreamId, cancellationToken);
        var dreamCategories = dreamCategoriesQuery?.ToList() ?? new List<Domain.Entity.DreamCategory>();
        var categories = await UnitOfWork.CategoryRepository.GetAsync<Domain.Entity.Category>(cancellationToken: cancellationToken)
            ?? new List<Domain.Entity.Category>();

        var res = dreamCategories.Join(
            categories,
            x => x.CategoryId,
            y => y.CategoryId,
            (x, y) => new CategoryResponseDto
            {
                CategoryId = y.CategoryId,
                Description = y.Description,
                Title = y.Title
            }
        );
        
        var answerDto = new DreamResponseDto
        {
            Id = dream.DreamId,
            Title = dream.Title,
            Description = dream.Description,
            ProducerId = dream.ProducerId,
            Rating = dream.Rating,

            Categories = res.ToList(),
            
            ImageBase64 = Convert.ToBase64String(imageBytes),
            ImageContentType = dreamImg.ContentType
        };

        await cacheService.SetAsync(request.DreamId.ToString() + nameof(Dream), answerDto);

        return answerDto;
    }
}