using Application.DTO;
using Application.DTO.ConsumerUser;
using MediatR;

namespace Application.UseCases.ConsumerUserAuth.ConsumerUserRegister;

public class ConsumerUserRegisterCommand(ConsumerUserRegisterDto dto) : IRequest
{
    public ConsumerUserRegisterDto Model { get; init; } = dto;
}