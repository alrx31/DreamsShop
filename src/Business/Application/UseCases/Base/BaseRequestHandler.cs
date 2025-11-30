using Application.DI;
using AutoMapper;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.Base;

public abstract class BaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    protected IUnitOfWork UnitOfWork => ServiceLocator.GetService<IUnitOfWork>();
    protected IMapper Mapper => ServiceLocator.GetService<IMapper>();

    
    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
