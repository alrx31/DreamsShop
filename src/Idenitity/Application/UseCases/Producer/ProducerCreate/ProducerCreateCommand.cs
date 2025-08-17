using Application.DTO.Producer;
using Application.UseCases.ProducerUserAuth.ProducerUserRegister;
using MediatR;

namespace Application.UseCases.Producer.ProducerCreate;

public record ProducerCreateCommand(ProducerCreateDTO Dto, ProducerUserRegisterCommand ProducerUserDto) : IRequest<Guid>;