using MediatR;

namespace Application.UseCases.ConsumerUser.ConsumerUserDelete;

public record ConsumerUserDeleteCommand(Guid ConsumerUserId) : IRequest;