using Application.DTO;
using MediatR;

namespace Application.UseCases.Dreams.DreamUpdate;

public record DreamUpdateCommand(Guid DreamId, DreamUpdateDto Dto) : IRequest;