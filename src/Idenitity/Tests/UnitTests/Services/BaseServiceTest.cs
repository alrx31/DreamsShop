using MediatR;

namespace Tests.UnitTests.Services;

public abstract class BaseServiceTest<T> where T:IRequest
{
    protected IRequestHandler<T> _handler { get; init; }
}