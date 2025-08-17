using Application.DTO.Producer;
using MediatR;

namespace Application.UseCases.Producer.ProducerUpdate;

public record ProducerUpdateCommand(ProducerCreateDTO Dto, Guid ProducerId) : IRequest;
