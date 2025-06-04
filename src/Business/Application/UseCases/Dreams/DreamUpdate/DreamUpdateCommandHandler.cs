using Application.Exceptions;
using Domain.IRepositories;
using Domain.IService;
using MediatR;

namespace Application.UseCases.Dreams.DreamUpdate;

public class DreamUpdateCommandHandler(
    IUnitOfWork unitOfWork,
    IHttpContextService httpContextService
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
        
        await unitOfWork.DreamRepository.UpdateAsync(dream, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}