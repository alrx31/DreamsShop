using MediatR;

namespace Application.UseCases.DreamCategory.DreamCategoryDelete;

public record DreamCategoryDeleteCommand(Guid DreamId, Guid CategoryId) : IRequest;