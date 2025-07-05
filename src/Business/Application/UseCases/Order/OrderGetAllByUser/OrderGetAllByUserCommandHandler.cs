using Application.DTO.Order;
using Application.Exceptions;
using AutoMapper;
using Domain.IRepositories;
using Domain.IService;
using MediatR;

namespace Application.UseCases.Order.OrderGetAllByUser;

public class OrderGetAllByUserCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IHttpContextService httpContextService
) : IRequestHandler<OrderGetAllByUserCommand, IEnumerable<OrderResponseDto>>
{
    public async Task<IEnumerable<OrderResponseDto>> Handle(OrderGetAllByUserCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextService.GetCurrentUserId();
        if (!userId.HasValue)
        {
            throw new UnauthorizedException("User is not authenticated.");
        }

        var orders = await unitOfWork.OrderRepository.GetOrdersByUser(userId.Value, request.StartIndex, request.Skip, cancellationToken);
        return mapper.Map<IEnumerable<OrderResponseDto>>(orders);
    }
}
