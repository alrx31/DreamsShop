using Application.DTO.Order;
using Application.Exceptions;
using AutoMapper;
using Domain.IRepositories;
using Domain.IService;
using MediatR;

namespace Application.UseCases.Order.OrderGetOne;

public class OrderGetOneCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IHttpContextService httpContextService
) : IRequestHandler<OrderGetOneCommand, OrderResponseDto>
{
    public async Task<OrderResponseDto> Handle(OrderGetOneCommand request, CancellationToken cancellationToken)
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

        return mapper.Map<OrderResponseDto>(order);
    }
}
