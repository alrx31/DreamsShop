using Application.DTO;
using Application.Exceptions;
using Domain.IRepositories;
using Domain.IService;
using MediatR;

namespace Application.UseCases.Dreams.DreamsGetOne;

public class DreamGetOneCommandHandler(
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService
    ) : IRequestHandler<DreamGetOneCommand, DreamResponseDto?>
{
    public async Task<DreamResponseDto?> Handle(DreamGetOneCommand request, CancellationToken cancellationToken)
    {
        var dream = await unitOfWork.DreamRepository.GetAsync([request.DreamId], cancellationToken);
        if (dream is null) throw new NotFoundException("Dream not found.");
        
        var dreamImg = await fileStorageService.DownloadFileAsync(dream.ImageFileName, cancellationToken);
        using var stream = new MemoryStream();
        await dreamImg.Content!.CopyToAsync(stream, cancellationToken);
        var imageBytes = stream.ToArray();
        
        var dreamCategories = await unitOfWork.DreamCategoryRepository.GetCategoriesByDreamIdAsync(dream.DreamId, cancellationToken);
        var categories = await unitOfWork.CategoryRepository.GetAllAsync(cancellationToken);

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
        
        return answerDto;
    }
}