using Application.DTO;
using MediatR;

namespace Application.UseCases.Dreams.DreamsGetOne;

public class DreamGetOneCommand : IRequest<DreamGetDto?>
{
    public required Guid DreamId { get; init; }
}