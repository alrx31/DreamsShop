using System.Text.Json;
using System.Text.Json.Nodes;
using Application.DTO;
using Application.Exceptions;
using Domain.Entity;
using Domain.IRepositories;
using Domain.IService;
using Domain.Model;
using MediatR;

namespace Application.UseCases.Dreams.DreamDelete;

public class DreamDeleteCommandHandler (
    IUnitOfWork unitOfWork,
    ICacheService<string, DreamResponseDto> cacheService,
    ICacheService<DreamCacheKey, List<DreamResponseDto>> allDreamCacheService
    ): IRequestHandler<DreamDeleteCommand>
{
    public async Task Handle(DreamDeleteCommand request, CancellationToken cancellationToken)
    {
        var dream = await unitOfWork.DreamRepository.GetAsync([request.DreamId], cancellationToken);
        if (dream is null) throw new NotFoundException("Dream not found.");

        var allCacheKey = new DreamCacheKey
        {
            StartIndex = DreamCacheKey.DefaultStartIndex,
            Count = DreamCacheKey.DefaultCount
        };

        await cacheService.RemoveAsync(request.DreamId.ToString() + nameof(Dream));
        await allDreamCacheService.RemoveAsync(allCacheKey);

        await unitOfWork.DreamRepository.DeleteAsync(dream, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}