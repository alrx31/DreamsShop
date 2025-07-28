using Application.DTO;
using MediatR;

namespace Application.UseCases.Dreams.DreamsGetOne;

public class DreamGetOneCommand : IRequest<DreamResponseDto?>
{
    public required Guid DreamId { get; init; }
}