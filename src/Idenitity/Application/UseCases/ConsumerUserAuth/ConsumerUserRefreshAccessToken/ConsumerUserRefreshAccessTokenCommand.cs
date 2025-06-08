using MediatR;

namespace Application.UseCases.ConsumerUserAuth.ConsumerUserRefreshAccessToken;

public record ConsumerUserRefreshAccessTokenCommand() : IRequest<string>;