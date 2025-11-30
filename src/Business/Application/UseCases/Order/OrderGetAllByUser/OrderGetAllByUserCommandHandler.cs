using Application.DTO.Order;
using Application.Exceptions;
using Application.UseCases.Base;
using Domain.IService;
using Domain.Specifications;

namespace Application.UseCases.Order.OrderGetAllByUser;

public class OrderGetAllByUserCommandHandler(
    IHttpContextService httpContextService,
    ICacheService<string, IEnumerable<OrderResponseDto>> cacheService
) : BaseRequestHandler<OrderGetAllByUserCommand, IEnumerable<OrderResponseDto>>
{
    public override async Task<IEnumerable<OrderResponseDto>> Handle(OrderGetAllByUserCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextService.GetCurrentUserId();
        if (!userId.HasValue)
        {
            throw new UnauthorizedException("User is not authenticated.");
        }

        var cachedOrders = await cacheService.GetAsync(userId.Value.ToString() + nameof(Order));
        if (cachedOrders is not null) return cachedOrders;

        var filter = new ValueSpecification<Domain.Entity.Order, Guid>(o => o.UserId, [userId.Value]);

        var orders = await UnitOfWork.OrderRepository
            .GetAsync<Domain.Entity.Order>(
                filter: filter.ToExpression(),
                skip: request.StartIndex,
                take: request.Skip,
                cancellationToken: cancellationToken);

        var mappedOrders = Mapper.Map<IEnumerable<OrderResponseDto>>(orders);

        await cacheService.SetAsync(userId.Value.ToString() + nameof(Order), mappedOrders);

        return mappedOrders;
    }
}
