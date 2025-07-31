using Application.DTO.Order;
using MediatR;

namespace Application.UseCases.Order.OrderGetOne;

public record OrderGetOneCommand(Guid Id) : IRequest<OrderResponseDto>;