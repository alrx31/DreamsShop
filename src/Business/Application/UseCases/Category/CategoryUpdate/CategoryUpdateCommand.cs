using Application.DTO;
using MediatR;

namespace Application.UseCases.Category.CategoryUpdate;

public record CategoryUpdateCommand(CategoryUpdateDto Dto, Guid CategoryId) : IRequest;