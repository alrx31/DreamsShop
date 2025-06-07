using MediatR;

namespace Application.UseCases.ConsumerUserAuth.ConsumerUserRefreshAccessToken;

public record ConsumerUserRefreshAccessTokenCommand(string ConsumerUserId) : IRequest<string>;