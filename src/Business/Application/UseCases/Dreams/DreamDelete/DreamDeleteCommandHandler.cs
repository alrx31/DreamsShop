using Application.DTO;
using Application.Exceptions;
using Domain.Entity;
using Domain.IRepositories;
using Domain.IService;
using MediatR;

namespace Application.UseCases.Dreams.DreamDelete;

public class DreamDeleteCommandHandler (
    IUnitOfWork unitOfWork,
    ICacheService<string, DreamResponseDto> cacheService
    ): IRequestHandler<DreamDeleteCommand>
{
    public async Task Handle(DreamDeleteCommand request, CancellationToken cancellationToken)
    {
        var dream = await unitOfWork.DreamRepository.GetAsync([request.DreamId], cancellationToken);
        if (dream is null) throw new NotFoundException("Dream not found.");

        await cacheService.RemoveAsync(request.DreamId.ToString() + nameof(Dream));

        await unitOfWork.DreamRepository.DeleteAsync(dream, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}