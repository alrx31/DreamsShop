using MediatR;

namespace Application.UseCases.Order.OrderGetOne;

public record OrderGetOneCommand(Guid Id) : IRequest<Domain.Entity.Order>;