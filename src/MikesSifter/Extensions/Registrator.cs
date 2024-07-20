using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MikesSifter.Filtering;
using MikesSifter.Paging;
using MikesSifter.Sorting;

namespace MikesSifter.Extensions;

public static class Registrator
{
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