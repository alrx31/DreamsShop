using Application.Exceptions;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.Producer.ProducerGet;

public class ProducerGetCommandHandler(
    IUnitOfWork unitOfWork
) : IRequestHandler<ProducerGetCommand, Domain.Entity.Producer>
{
    public async Task<Domain.Entity.Producer> Handle(ProducerGetCommand request, CancellationToken cancellationToken)
    {
        var producer = await unitOfWork.ProducerRepository.GetAsync(request.Id, cancellationToken);
        if (producer is null)

        {
            throw new NotFoundException($"Producer with ID {request.Id} not found.");
        }

        return producer;
    }
}
