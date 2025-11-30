using MediatR;

namespace Application.UseCases.Dreams.DreamDelete;

public record DreamDeleteCommand(Guid DreamId) : IRequest<Unit>;