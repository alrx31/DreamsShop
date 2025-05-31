using MediatR;

namespace Application.UseCases.Dreams.DreamDelete;

public class DreamDeleteCommand : IRequest
{
    public Guid DreamId { get; set; }
}