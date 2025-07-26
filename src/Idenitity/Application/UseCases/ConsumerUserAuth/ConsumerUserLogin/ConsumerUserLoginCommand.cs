using Application.DTO;
using Application.DTO.ConsumerUser;
using MediatR;

namespace Application.UseCases.ConsumerUserAuth.ConsumerUserLogin;

public record ConsumerUserLoginCommand
    (ConsumerUserLoginDto ConsumerUserLoginDto) : IRequest<ConsumerUserAuthResponseDto>;