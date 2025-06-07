using Application.DTO;
using MediatR;

namespace Application.UseCases.Category.CategoryCreate;

public record CategoryAddCommand(CategoryCreateDto Dto) : IRequest;