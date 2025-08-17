using Application.Exceptions;
using Domain.IRepositories;
using Domain.IServices;
using MediatR;

namespace Application.UseCases.Producer.ProducerUpdate;

public class ProducerUpdateCommandHandler(
    IUnitOfWork unitOfWork,
    IHttpContextService httpContextService
) : IRequestHandler<ProducerUpdateCommand>
{
    public async Task Handle(ProducerUpdateCommand request, CancellationToken cancellationToken)
    {
        var producer = await unitOfWork.ProducerRepository.GetAsync(request.ProducerId, cancellationToken);
        if (producer is null) throw new NotFoundException("Producer not found");

        var currentUserId = httpContextService.GetCurrentUserId() ?? throw new UnauthorizedException("User not found");
        var user = await unitOfWork.ProducerUserRepository.GetAsync(currentUserId, cancellationToken);

        if(user is null || user.ProducerId != producer.Id || user.Role != Domain.Entity.Roles.ProducerAdmin)
            throw new ForbiddenException("You do not have permission to update this producer");

        producer.Title = request.Dto.Title;
        producer.Description = request.Dto.Description;

        await unitOfWork.ProducerRepository.UpdateAsync(producer, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
