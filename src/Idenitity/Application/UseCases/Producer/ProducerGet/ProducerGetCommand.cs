using MediatR;

namespace Application.UseCases.Producer.ProducerGet;

public record ProducerGetCommand(Guid Id) : IRequest<Domain.Entity.Producer>;