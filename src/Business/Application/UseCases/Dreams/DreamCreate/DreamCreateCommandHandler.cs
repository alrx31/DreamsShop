using Application.DTO;
using Application.Exceptions;
using Application.UseCases.Base;
using Domain.Entity;
using Domain.IService;  
using Domain.Model;
using Microsoft.Extensions.Options;
using Shared.Configuration;

namespace Application.UseCases.Dreams.DreamCreate;

public class DreamCreateCommandHandler(
        IHttpContextService httpContextService,
        IFileStorageService fileStorageService,
        ICacheService<DreamCacheKey, List<DreamResponseDto>> cacheService,
        IOptions<BaseDreamImageConfiguration> baseDreamImageConfiguration
    ) : BaseRequestHandler<DreamCreateCommand, Guid>
{
    public override async Task<Guid> Handle(DreamCreateCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextService.GetCurrentUserId();
        if(userId is null) throw new UnauthorizedException("Invalid user id.");
        
        request.ProducerId = userId;

        var dreamModel = Mapper.Map<Dream>(request);
        
        var image = request.Image;
        if (image is not null && image.Content is not null)
        {
            var objectName = await fileStorageService.UploadFileAsync(image, cancellationToken);
            
            dreamModel.ImageFileName = objectName;
        }
        else
        {
            dreamModel.ImageFileName = baseDreamImageConfiguration.Value.DefaultDreamImage;
        }
        
        var id = (await UnitOfWork.DreamRepository.AddAsync(
            dreamModel,
            cancellationToken)).DreamId;
 
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        await cacheService.RemoveAsync(new DreamCacheKey());

        return id;
    }
}