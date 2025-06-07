using Application.DTO;
using MediatR;

namespace Application.UseCases.Category.CategoryAdd;

public record CategoryAddCommand(CategoryCreateDto Dto) : IRequest;