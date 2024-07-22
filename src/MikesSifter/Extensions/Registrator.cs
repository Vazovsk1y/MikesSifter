using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MikesSifter.Filtering;
using MikesSifter.Paging;
using MikesSifter.Sorting;

namespace MikesSifter.Extensions;

public static class Registrator
{
    /// <summary>
    /// Adds the specified sifter implementation and its related services to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TSifter">The type of the sifter implementation that derives from <see cref="MikesSifter"/>.</typeparam>
    /// <param name="serviceCollection">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="lifetime">The <see cref="ServiceLifetime"/> of the services. The default is <see cref="ServiceLifetime.Scoped"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    /// <remarks>
    /// This method registers the sifter implementation and its related services
    /// (<see cref="IMikesSifter"/>, <see cref="IFilteringManager"/>, <see cref="ISortingManager"/>, <see cref="IPagingManager"/>)
    /// with the specified service lifetime.
    /// </remarks>
    public static IServiceCollection AddSifter<TSifter>(this IServiceCollection serviceCollection, ServiceLifetime lifetime = ServiceLifetime.Scoped) where TSifter : MikesSifter
    {
        serviceCollection.TryAdd(new ServiceDescriptor(typeof(TSifter), typeof(TSifter), lifetime));
        serviceCollection.TryAdd(new ServiceDescriptor(typeof(IMikesSifter), provider => provider.GetRequiredService<TSifter>(), lifetime));
        serviceCollection.TryAdd(new ServiceDescriptor(typeof(IFilteringManager), provider => provider.GetRequiredService<TSifter>(), lifetime));
        serviceCollection.TryAdd(new ServiceDescriptor(typeof(ISortingManager), provider => provider.GetRequiredService<TSifter>(), lifetime));
        serviceCollection.TryAdd(new ServiceDescriptor(typeof(IPagingManager), provider => provider.GetRequiredService<TSifter>(), lifetime));

        return serviceCollection;
    }
}