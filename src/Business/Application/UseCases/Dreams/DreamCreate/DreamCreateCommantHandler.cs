using Application.Exceptions;
using AutoMapper;
using Domain.Entity;
using Domain.IRepositories;
using Domain.IService;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.Configuration;

namespace Application.UseCases.Dreams.DreamCreate;

public class DreamCreateCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IHttpContextService httpContextService,
        IFileStorageService fileStorageService,
        IOptions<BaseDreamImageConfiguration> baseDreamImageConfiguration
    ) : IRequestHandler<DreamCreateCommand, Guid>
{
    public async Task<Guid> Handle(DreamCreateCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextService.GetCurrentUserId();
        if(userId is null) throw new UnauthorizedException("Invalid user id.");
        
        request.ProducerId = userId;

        var dreamModel = mapper.Map<Dream>(request);
        
        var image = request.Image;
        if (image is not null)
        {
            var objectName = await fileStorageService.UploadFileAsync(image, cancellationToken);
            
            dreamModel.ImageFileName = objectName;
        }
        else
        {
            dreamModel.ImageFileName = baseDreamImageConfiguration.Value.DefaultDreamImage;
        }
        
        var id = await unitOfWork.DreamRepository.AddAsync(
            dreamModel,
            cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return id;
    }
}