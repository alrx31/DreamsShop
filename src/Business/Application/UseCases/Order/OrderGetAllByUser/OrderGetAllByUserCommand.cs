using Application.DTO.Order;
using MediatR;

namespace Application.UseCases.Order.OrderGetAllByUser;

public record OrderGetAllByUserCommand(int StartIndex, int Skip) : IRequest<IEnumerable<OrderResponseDto>>;
