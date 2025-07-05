using Application.DTO.ProducerUser;
using MediatR;

namespace Application.UseCases.ProducerUserAuth.ProducerUserRegister;

public record ProducerUserRegisterCommand(ProducerUserRegisterDto Dto) : IRequest;