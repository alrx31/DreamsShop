using Application.DTO.Order;
using Application.Exceptions;
using Application.UseCases.Base;
using Domain.Entity;
using Domain.IService;
using Domain.Specifications;

namespace Application.UseCases.Order.CreateOrder;

public class OrderCreateCommandHandler(
        IHttpContextService httpContextService,
        ICacheService<string, IEnumerable<OrderResponseDto>> cacheService
    ) : BaseRequestHandler<OrderCreateCommand, Guid>
{
    public override async Task<Guid> Handle(OrderCreateCommand request, CancellationToken cancellationToken)
    {
        var filter = new ValueSpecification<Dream, Guid>(d=>d.DreamId, request.DTO.DreamIds?.ToArray() ?? Array.Empty<Guid>());

        var dreams = await UnitOfWork.DreamRepository.GetAsync<Dream>(
            filter: filter.ToExpression(),
            cancellationToken: cancellationToken
            );
        
        if (dreams is null || dreams.Count != (request.DTO.DreamIds?.Count ?? 0))
        {
            throw new NotFoundException("Some dreams not found for order.");
        }

        var order = new Domain.Entity.Order
        {
            CreatedAt = DateTime.UtcNow
        };

        var userId = httpContextService.GetCurrentUserId();
        if (userId is null)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        order.UserId = userId.Value;
        order.OrderDreams = dreams.Select(dream => new Domain.Entity.OrderDream
        {
            DreamId = dream.DreamId
        });

        var orderId = (await UnitOfWork.OrderRepository.AddAsync(order, cancellationToken)).OrderId;
        if (orderId == Guid.Empty)
        {
            throw new BadRequestException("Failed to create order.");
        }

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        await cacheService.RemoveAsync(userId.Value.ToString() + nameof(Order));
        return orderId;
    }
}