using Application.Exceptions;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.Order.CreateOrder;

public class CreateOrderCommandHandler(
        IUnitOfWork unitOfWork
    ): IRequestHandler<CreateOrderCommand, Guid>
{
    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
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