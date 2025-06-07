using Application.DTO;
using Application.DTO.ConsumerUser;
using MediatR;

namespace Application.UseCases.ConsumerUserAuth.ConsumerUserRegister;

public record ConsumerUserRegisterCommand(ConsumerUserRegisterDto Dto) : IRequest;