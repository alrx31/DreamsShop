using Application.DTO;
using MediatR;

namespace Application.UseCases.Commands;

public class ConsumerUserLoginCommand
    (ConsumerUserLoginDto consumerUserLoginDto) : IRequest<ConsumerUserAuthResponseDto>
{
    public ConsumerUserLoginDto ConsumerUserLoginDto { get; } = consumerUserLoginDto;
}