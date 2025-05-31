using MediatR;

namespace Application.UseCases.Category.CategoryGet;

public record CategoryGetCommand(Guid CategoryId) : IRequest<Domain.Entity.Category?>;