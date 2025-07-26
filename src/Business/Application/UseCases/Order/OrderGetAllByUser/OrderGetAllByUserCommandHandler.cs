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
    IHttpContextService httpContextService,
    ICacheService<string, IEnumerable<OrderResponseDto>> cacheService
) : IRequestHandler<OrderGetAllByUserCommand, IEnumerable<OrderResponseDto>>
{
    public async Task<IEnumerable<OrderResponseDto>> Handle(OrderGetAllByUserCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextService.GetCurrentUserId();
        if (!userId.HasValue)
        {
            throw new UnauthorizedException("User is not authenticated.");
        }

        var cachedOrders = await cacheService.GetAsync(userId.Value.ToString() + nameof(Order));
        if (cachedOrders is not null) return cachedOrders;

        var orders = await unitOfWork.OrderRepository.GetOrdersByUser(userId.Value, request.StartIndex, request.Skip, cancellationToken);
        var mappedOrders = mapper.Map<IEnumerable<OrderResponseDto>>(orders);

        await cacheService.SetAsync(userId.Value.ToString() + nameof(Order), mappedOrders);

        return mappedOrders;
    }
}
