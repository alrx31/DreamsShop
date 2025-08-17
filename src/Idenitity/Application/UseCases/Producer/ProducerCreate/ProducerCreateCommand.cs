using Application.DTO.Producer;
using MediatR;

namespace Application.UseCases.Producer.ProducerCreate;

public record ProducerCreateCommand(ProducerCreateDTO Dto) : IRequest<Guid>;