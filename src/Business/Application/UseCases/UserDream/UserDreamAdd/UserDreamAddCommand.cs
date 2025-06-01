using MediatR;

namespace Application.UseCases.UserDream.UserDreamAdd;

public record UserDreamAddCommand(Guid DreamId) : IRequest;