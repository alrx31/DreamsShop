using MediatR;

namespace Application.UseCases.Producer.ProducerDelete;

public record ProducerDeleteCommand(Guid ProducerId) : IRequest;
