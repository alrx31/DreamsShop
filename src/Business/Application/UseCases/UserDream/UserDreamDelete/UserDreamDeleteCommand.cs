using MediatR;

namespace Application.UseCases.UserDream.UserDreamDelete;

public record UserDreamDeleteCommand(Guid DreamId) : IRequest;