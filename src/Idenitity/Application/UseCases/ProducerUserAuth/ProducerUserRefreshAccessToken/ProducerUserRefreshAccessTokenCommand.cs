using MediatR;

namespace Application.UseCases.ProducerUserAuth.ProducerUserRefreshAccessToken;

public record ProducerUserRefreshAccessTokenCommand() : IRequest<string>;