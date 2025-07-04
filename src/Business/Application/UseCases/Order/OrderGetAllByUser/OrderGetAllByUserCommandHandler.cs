using Application.Exceptions;
using Domain.IRepositories;
using Domain.IService;
using MediatR;

namespace Application.UseCases.Order.OrderGetAllByUser;

public class OrderGetAllByUserCommandHandler(
    IUnitOfWork unitOfWork,
    IHttpContextService httpContextService
) : IRequestHandler<OrderGetAllByUserCommand, IEnumerable<Domain.Entity.Order>>
{
    public async Task<IEnumerable<Domain.Entity.Order>> Handle(OrderGetAllByUserCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextService.GetCurrentUserId();
        if (userId is null)
        {
            throw new UnauthorizedException("User is not authenticated.");
        }
        
        return await unitOfWork.OrderRepository.GetOrdersByUser(userId.Value, cancellationToken);
    }
}
