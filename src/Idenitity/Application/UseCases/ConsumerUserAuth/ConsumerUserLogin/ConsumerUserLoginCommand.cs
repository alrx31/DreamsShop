using Application.DTO;
using Application.DTO.ConsumerUser;
using MediatR;

namespace Application.UseCases.ConsumerUserAuth.ConsumerUserLogin;

public class ConsumerUserLoginCommand
    (ConsumerUserLoginDto consumerUserLoginDto) : IRequest<ConsumerUserAuthResponseDto>
{
    public ConsumerUserLoginDto ConsumerUserLoginDto { get; } = consumerUserLoginDto;
}