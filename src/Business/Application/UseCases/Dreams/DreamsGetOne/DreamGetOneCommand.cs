using Domain.Entity;
using MediatR;

namespace Application.UseCases.Dreams.DreamsGetOne;

public class DreamGetOneCommand : IRequest<Dream?>
{
    public required Guid DreamId { get; init; }
}