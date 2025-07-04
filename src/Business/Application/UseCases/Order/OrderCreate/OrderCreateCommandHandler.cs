using Application.Exceptions;
using Domain.IRepositories;
using Domain.IService;
using MediatR;

namespace Application.UseCases.Order.CreateOrder;

public class OrderCreateCommandHandler(
        IUnitOfWork unitOfWork,
        IHttpContextService httpContextService
    ) : IRequestHandler<OrderCreateCommand, Guid>
{
    public async Task<Guid> Handle(OrderCreateCommand request, CancellationToken cancellationToken)
    {
        var dreams = await unitOfWork.DreamRepository.GetRangeAsync(request.DTO.DreamIds ?? [], cancellationToken);
        if (dreams.Count() != (request.DTO.DreamIds?.Count ?? 0) || dreams is null)
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

        var orderId = await unitOfWork.OrderRepository.AddAsync(order, cancellationToken);
        if (orderId == Guid.Empty)
        {
            throw new BadRequestException("Failed to create order.");
        }

        foreach (var dream in dreams)
        {
            var orderDream = new Domain.Entity.OrderDream
            {
                OrderId = orderId,
                DreamId = dream.DreamId
            };

            var orderDreamId = await unitOfWork.OrderDreamRepository.AddAsync(orderDream, cancellationToken);
            if (orderDreamId == Guid.Empty)
            {
                throw new BadRequestException("Failed to add dream to order.");
            }
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return orderId;
    }
}