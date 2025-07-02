using Application.DTO;
using MediatR;

namespace Application.UseCases.Order.CreateOrder;

public record CreateOrderCommand(CreateOrderDTO DTO) : IRequest<Guid>;