using Application.DTO;
using Application.Exceptions;
using Application.UseCases.Base;
using Domain.Entity;
using Domain.IService;
using Domain.Model;
using MediatR;

namespace Application.UseCases.Dreams.DreamDelete;

public class DreamDeleteCommandHandler (
    ICacheService<string, DreamResponseDto> cacheService,
    ICacheService<DreamCacheKey, List<DreamResponseDto>> allDreamCacheService,
    IHttpContextService httpContextService
    ): BaseRequestHandler<DreamDeleteCommand, Unit>
{
    public override async Task<Unit> Handle(DreamDeleteCommand request, CancellationToken cancellationToken)
    {
        var dream = await UnitOfWork.DreamRepository.GetAsync([request.DreamId], cancellationToken);
        if (dream is null) throw new NotFoundException("Dream not found.");

        var currentUserId = httpContextService.GetCurrentUserId();
        if (currentUserId != dream.ProducerId) 
            throw new UnauthorizedException("You are not authorized to delete this dream.");

        await cacheService.RemoveAsync(request.DreamId.ToString() + nameof(Dream));
        await allDreamCacheService.RemoveAsync(new DreamCacheKey());

        await UnitOfWork.DreamRepository.DeleteAsync(dream, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}