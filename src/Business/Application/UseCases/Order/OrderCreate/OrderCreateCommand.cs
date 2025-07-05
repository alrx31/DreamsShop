using Application.DTO;
using MediatR;

namespace Application.UseCases.Order.CreateOrder;
public record OrderCreateCommand(OrderCreateDto DTO) : IRequest<Guid>;
