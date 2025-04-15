using Microsoft.Extensions.DependencyInjection;

namespace ApiCaller.Extensions;

public static class IServiceCollectionExtension
{
    public static void AddApiClient(this IServiceCollection services, ApiClientOptions? options = null, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        services.AddHttpClient();
        switch (serviceLifetime)
        {
            case ServiceLifetime.Singleton:
                services.AddSingleton(provider =>
                {
                    var httpClient = provider.GetRequiredService<HttpClient>();
                    return new ApiClient(httpClient, options);
                });
                break;
            case ServiceLifetime.Scoped:
                services.AddScoped(provider =>
                {
                    var httpClient = provider.GetRequiredService<HttpClient>();
                    return new ApiClient(httpClient, options);
                });
                break;
            case ServiceLifetime.Transient:
                services.AddTransient(provider =>
                {
                    var httpClient = provider.GetRequiredService<HttpClient>();
                    return new ApiClient(httpClient, options);
                });
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null);
        }
    }

    public static void AddApiClient<TDerived>(this IServiceCollection services, ApiClientOptions? options = null, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where TDerived : ApiClient
    {
        services.AddHttpClient();
        switch (serviceLifetime)
        {
            case ServiceLifetime.Singleton:
                services.AddSingleton(provider =>
                {
                    var httpClient = provider.GetRequiredService<HttpClient>();
                    object instance = Activator.CreateInstance(typeof(TDerived), args: [httpClient, options])!;
                    return (TDerived)instance;
                });
                break;
            case ServiceLifetime.Scoped:
                services.AddScoped(provider =>
                {
                    var httpClient = provider.GetRequiredService<HttpClient>();
                    object instance = Activator.CreateInstance(typeof(TDerived), args: [httpClient, options])!;
                    return (TDerived)instance;
                });
                break;
            case ServiceLifetime.Transient:
                services.AddTransient(provider =>
                {
                    var httpClient = provider.GetRequiredService<HttpClient>();
                    object instance = Activator.CreateInstance(typeof(TDerived), args: [httpClient, options])!;
                    return (TDerived)instance;
                });
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null);
        }
    }

    public static void AddKeyedApiClient(this IServiceCollection services, string serviceKey, ApiClientOptions? options = null, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        services.AddHttpClient();
        switch (serviceLifetime)
        {
            case ServiceLifetime.Singleton:
                services.AddKeyedSingleton(serviceKey, (provider, _) =>
                {
                    var httpClient = provider.GetRequiredService<HttpClient>();
                    return new ApiClient(httpClient, options);
                });
                break;
            case ServiceLifetime.Scoped:
                services.AddKeyedScoped(serviceKey, (provider, _) =>
                {
                    var httpClient = provider.GetRequiredService<HttpClient>();
                    return new ApiClient(httpClient, options);
                });
                break;
            case ServiceLifetime.Transient:
                services.AddKeyedTransient(serviceKey, (provider, _) =>
                {
                    var httpClient = provider.GetRequiredService<HttpClient>();
                    return new ApiClient(httpClient, options);
                });
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null);
        }
    }
}