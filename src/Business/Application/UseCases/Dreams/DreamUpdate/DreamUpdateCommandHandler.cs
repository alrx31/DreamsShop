using Application.DTO;
using Application.Exceptions;
using Domain.Entity;
using Domain.IRepositories;
using Domain.IService;
using Domain.Model;
using MediatR;

namespace Application.UseCases.Dreams.DreamUpdate;

public class DreamUpdateCommandHandler(
    IUnitOfWork unitOfWork,
    IHttpContextService httpContextService,
    IFileStorageService fileStorageService,
    ICacheService<string, DreamResponseDto> cacheService,
    ICacheService<DreamCacheKey, List<DreamResponseDto>> allDreamCacheService
    ) : IRequestHandler<DreamUpdateCommand>
{
    public async Task Handle(DreamUpdateCommand request, CancellationToken cancellationToken)
    {
        var dream = await unitOfWork.DreamRepository.GetAsync([request.DreamId], cancellationToken);
        if (dream is null) throw new NotFoundException("Dream not found.");

        var currentUser = httpContextService.GetCurrentUserId();
        
        if(dream.ProducerId != currentUser) throw new ForbiddenException("You do not have permission to update dream.");
        
        if (!string.IsNullOrWhiteSpace(request.Dto.Title))
        {
            dream.Title = request.Dto.Title;
        }

        if (!string.IsNullOrWhiteSpace(request.Dto.Description))
        {
            dream.Description = request.Dto.Description;
        }

        var image = request.Dto.Image;
        if (image is not null && image.Content is not null)
        {
            var objectName = await fileStorageService.UploadFileAsync(image, cancellationToken);
            
            dream.ImageFileName = objectName;
        }

        await allDreamCacheService.RemoveAsync(new DreamCacheKey());
        await cacheService.RemoveAsync(request.DreamId.ToString() + nameof(Dream));
        
        await unitOfWork.DreamRepository.UpdateAsync(dream, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}