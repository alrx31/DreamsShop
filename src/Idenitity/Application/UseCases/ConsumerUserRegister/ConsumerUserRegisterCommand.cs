using Application.DTO;
using MediatR;

namespace Application.UseCases.ConsumerUserRegister;

public class ConsumerUserRegisterCommand(ConsumerUserRegisterDto dto) : IRequest
{
    public ConsumerUserRegisterDto Model { get; init; } = dto;
}