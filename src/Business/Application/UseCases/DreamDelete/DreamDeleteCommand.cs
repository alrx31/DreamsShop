using MediatR;

namespace Application.UseCases.DreamDelete;

public class DreamDeleteCommand : IRequest
{
    public Guid DreamId { get; set; }
}