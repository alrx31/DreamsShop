using Application.Exceptions;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.DreamDelete;

public class DreamDeleteCommandHandler (
    IUnitOfWork unitOfWork
    ): IRequestHandler<DreamDeleteCommand>
{
    public async Task Handle(DreamDeleteCommand request, CancellationToken cancellationToken)
    {
        var dream = await unitOfWork.DreamRepository.GetAsync(request.DreamId, cancellationToken);
        if (dream is null) throw new NotFoundException("Dream not found.");
        
        await unitOfWork.DreamRepository.DeleteAsync(dream, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}