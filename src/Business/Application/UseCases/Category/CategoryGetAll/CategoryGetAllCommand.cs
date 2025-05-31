using MediatR;

namespace Application.UseCases.Category.CategoryGetAll;

public record CategoryGetAllCommand : IRequest<List<Domain.Entity.Category>>;