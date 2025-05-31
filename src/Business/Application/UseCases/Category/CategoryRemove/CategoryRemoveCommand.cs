using MediatR;

namespace Application.UseCases.Category.CategoryRemove;

public record CategoryRemoveCommand(Guid CategoryId) : IRequest;