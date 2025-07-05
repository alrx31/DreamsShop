using Application.Exceptions;
using Domain.IRepositories;
using Domain.IService;
using MediatR;

namespace Application.UseCases.Order.OrderGetOne;

public class OrderGetOneCommandHandler(
    IUnitOfWork unitOfWork,
    IHttpContextService httpContextService
) : IRequestHandler<OrderGetOneCommand, Domain.Entity.Order>
{
    public async Task<Domain.Entity.Order> Handle(OrderGetOneCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.OrderRepository.GetAsync([request.Id], cancellationToken);
        var userId = httpContextService.GetCurrentUserId();

        if (order is null)
        {
            throw new NotFoundException("Order not found.");
        }

        if (order.UserId != userId)
        { 
            throw new ForbiddenException("You do not have permission to access this order.");
        }

        return order;
    }
}
