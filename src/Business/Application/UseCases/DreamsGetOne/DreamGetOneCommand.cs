using Domain.Entity;
using MediatR;

namespace Application.UseCases.DreamsGetOne;

public class DreamGetOneCommand : IRequest<Dream?>
{
    public required Guid DreamId { get; set; }
}