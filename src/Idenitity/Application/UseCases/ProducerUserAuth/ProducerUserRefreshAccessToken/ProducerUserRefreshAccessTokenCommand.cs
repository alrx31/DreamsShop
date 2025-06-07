using MediatR;

namespace Application.UseCases.ProducerUserAuth.ProducerUserRefreshAccessToken;

public record ProducerUserRefreshAccessTokenCommand(string ProducerUserId) : IRequest<string>;