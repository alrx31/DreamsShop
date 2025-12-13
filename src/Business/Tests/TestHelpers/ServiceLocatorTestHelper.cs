using System;
using Application.DI;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.TestHelpers;

/// <summary>
/// Ensures handlers that rely on ServiceLocator resolve mocked services.
/// </summary>
public static class ServiceLocatorTestHelper
{
    public static ServiceLocatorTestScope UseServiceLocator(Action<IServiceCollection> configureServices)
    {
        var services = new ServiceCollection();
        configureServices(services);

        var provider = services.BuildServiceProvider();
        var previous = GetCurrentProvider();
        ServiceLocator.ServiceProvider = provider;

        return new ServiceLocatorTestScope(provider, previous);
    }

    public static ServiceLocatorTestScope UseServiceLocator(params (Type ServiceType, object Implementation)[] registrations)
    {
        return UseServiceLocator(services =>
        {
            foreach (var (serviceType, implementation) in registrations)
            {
                services.AddSingleton(serviceType, implementation);
            }
        });
    }

    public sealed class ServiceLocatorTestScope : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly IServiceProvider? _previousProvider;

        public ServiceLocatorTestScope(ServiceProvider serviceProvider, IServiceProvider? previousProvider)
        {
            _serviceProvider = serviceProvider;
            _previousProvider = previousProvider;
        }

        public void Dispose()
        {
            if (_previousProvider is not null)
            {
                ServiceLocator.ServiceProvider = _previousProvider;
            }
            else
            {
                ServiceLocator.ServiceProvider = new ServiceCollection().BuildServiceProvider();
            }

            _serviceProvider.Dispose();
        }
    }

    private static IServiceProvider? GetCurrentProvider()
    {
        try
        {
            return ServiceLocator.ServiceProvider;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }
}
