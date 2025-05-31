using Application.DTO.ProducerUser;
using MediatR;

namespace Application.UseCases.ProducerUserAuth.ProducerUserLogin;

public record ProducerUserLoginCommand(ProducerUserLoginDto Dto) :IRequest<ProducerUserAuthResponseDto>;