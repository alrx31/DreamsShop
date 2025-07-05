using MediatR;

namespace Application.UseCases.Order.OrderGetAllByUser;

public record OrderGetAllByUserCommand : IRequest<IEnumerable<Domain.Entity.Order>>;
