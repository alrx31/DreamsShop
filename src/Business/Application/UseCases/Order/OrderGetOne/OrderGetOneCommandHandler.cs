using Application.DTO.Order;
using Application.Exceptions;
using Application.UseCases.Base;
using Domain.IService;
using Domain.Specifications;

namespace Application.UseCases.Order.OrderGetOne;

public class OrderGetOneCommandHandler(
    IHttpContextService httpContextService
) : BaseRequestHandler<OrderGetOneCommand, OrderResponseDto>
{
    public override async Task<OrderResponseDto> Handle(OrderGetOneCommand request, CancellationToken cancellationToken)
    {
        var filter = new IdsSpecification<Domain.Entity.Order, Guid>(o=>o.OrderId,  [request.Id]);

        var order = (await UnitOfWork.OrderRepository
            .GetAsync<Domain.Entity.Order>(
                filter: filter.ToExpression(),
                cancellationToken: cancellationToken))
            .SingleOrDefault();

        var userId = httpContextService.GetCurrentUserId();

        if (order is null)
        {
            throw new NotFoundException("Order not found.");
        }

        if (order.UserId != userId)
        { 
            throw new ForbiddenException("You do not have permission to access this order.");
        }

        return Mapper.Map<OrderResponseDto>(order);
    }
}
