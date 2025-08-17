using Application.Exceptions;
using Domain.IRepositories;
using Domain.IServices;
using MediatR;

namespace Application.UseCases.Producer.ProducerDelete;

public class ProducerDeleteCommandHandler(
    IUnitOfWork unitOfWork,
    IHttpContextService httpContextService
) : IRequestHandler<ProducerDeleteCommand>
{
    public async Task Handle(ProducerDeleteCommand request, CancellationToken cancellationToken)
    {
        var producer = await unitOfWork.ProducerRepository.GetAsync(request.ProducerId, cancellationToken);
        if (producer == null) throw new NotFoundException("Producer not found");

        var currentUserId = httpContextService.GetCurrentUserId() ?? throw new UnauthorizedException("User not found");
        var user = await unitOfWork.ProducerUserRepository.GetAsync(currentUserId, cancellationToken);
        if (user is null) throw new UnauthorizedException("User not found");
        


        if (user.ProducerId != producer.Id || user.Role != Domain.Entity.Roles.ProducerAdmin)
            throw new ForbiddenException("You do not have permission to delete this producer");


        await unitOfWork.ProducerRepository.DeleteAsync(producer, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
