using Application.DTO;
using MediatR;

namespace Application.UseCases.ConsumerUserLogin;

public class ConsumerUserLoginCommand
    (ConsumerUserLoginDto consumerUserLoginDto) : IRequest<ConsumerUserAuthResponseDto>
{
    public ConsumerUserLoginDto ConsumerUserLoginDto { get; } = consumerUserLoginDto;
}