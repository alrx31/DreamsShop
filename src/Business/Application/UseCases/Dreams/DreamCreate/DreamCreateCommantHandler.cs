using Application.Exceptions;
using AutoMapper;
using Domain.Entity;
using Domain.IRepositories;
using Domain.IService;
using MediatR;

namespace Application.UseCases.Dreams.DreamCreate;

public class DreamCreateCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IHttpContextService httpContextService,
        IFileStorageService fileStorageService
    ) : IRequestHandler<DreamCreateCommand>
{
    public async Task Handle(DreamCreateCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextService.GetCurrentUserId();
        if(userId is null) throw new UnauthorizedException("Invalid user id.");
        
        request.Dto.ProducerId = userId;

        var dreamModel = mapper.Map<Dream>(request);
        
        var image = request.Dto.Image;
        if (image is not null)
        {
            var objectName = await fileStorageService.UploadFileAsync(image, cancellationToken);
            
            dreamModel.ImageFileName = objectName;
        }
        else
        {
            dreamModel.ImageFileName = null;
        }     
        
        await unitOfWork.DreamRepository.AddAsync(
            dreamModel,
            cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}