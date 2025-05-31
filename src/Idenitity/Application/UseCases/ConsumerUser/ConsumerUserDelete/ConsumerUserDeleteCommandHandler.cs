using Application.Exceptions;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.ConsumerUser.ConsumerUserDelete;

public class ConsumerUserDeleteCommandHandler(
    IUnitOfWork unitOfWork
    ) : IRequestHandler<ConsumerUserDeleteCommand>
{
    public async Task Handle(ConsumerUserDeleteCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.ConsumerUserRepository.GetAsync(request.ConsumerUserId, cancellationToken);
        if (user is null) throw new NotFoundException("Consumer user not found");
        
        await unitOfWork.ConsumerUserRepository.DeleteAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}