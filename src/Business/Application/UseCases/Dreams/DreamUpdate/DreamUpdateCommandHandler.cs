using Application.DTO;
using Application.Exceptions;
using Application.UseCases.Base;
using Domain.Entity;
using Domain.IService;
using Domain.Model;
using MediatR;

namespace Application.UseCases.Dreams.DreamUpdate;

public class DreamUpdateCommandHandler(
    IHttpContextService httpContextService,
    IFileStorageService fileStorageService,
    ICacheService<string, DreamResponseDto> cacheService,
    ICacheService<DreamCacheKey, List<DreamResponseDto>> allDreamCacheService
    ) : BaseRequestHandler<DreamUpdateCommand, Unit> 
{
    public override async Task<Unit> Handle(DreamUpdateCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto ?? new DreamUpdateDto();

        var dream = await UnitOfWork.DreamRepository.GetAsync([request.DreamId], cancellationToken);
        if (dream is null) throw new NotFoundException("Dream not found.");

        var currentUser = httpContextService.GetCurrentUserId();
        
        if(dream.ProducerId != currentUser) throw new ForbiddenException("You do not have permission to update dream.");
        
        if (!string.IsNullOrWhiteSpace(dto.Title))
        {
            dream.Title = dto.Title;
        }

        if (!string.IsNullOrWhiteSpace(dto.Description))
        {
            dream.Description = dto.Description;
        }

        var image = dto.Image;
        if (image is not null && image.Content is not null)
        {
            var objectName = await fileStorageService.UploadFileAsync(image, cancellationToken);
            
            dream.ImageFileName = objectName;
        }

        await allDreamCacheService.RemoveAsync(new DreamCacheKey());
        await cacheService.RemoveAsync(request.DreamId.ToString() + nameof(Dream));
        
        await UnitOfWork.DreamRepository.UpdateAsync(dream, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}