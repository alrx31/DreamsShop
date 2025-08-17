using AutoMapper;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.Producer.ProducerCreate;

public class ProducerCreateCommandHandler(
    IMapper mapper,
    IUnitOfWork unitOfWork
) : IRequestHandler<ProducerCreateCommand, Guid>
{
    public async Task<Guid> Handle(ProducerCreateCommand request, CancellationToken cancellationToken)
    {
        var producer = mapper.Map<Domain.Entity.Producer>(request.Dto);

        await unitOfWork.ProducerRepository.AddAsync(producer, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return producer.Id;
    }
}
