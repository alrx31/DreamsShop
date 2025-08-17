using MediatR;

namespace Application.UseCases.DreamCategory.DreamCategoryAdd;

public record DreamCategoryAddCommand(Guid DreamId, Guid CategoryId) : IRequest;