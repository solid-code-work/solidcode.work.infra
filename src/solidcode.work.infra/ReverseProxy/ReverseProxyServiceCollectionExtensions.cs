using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace solidcode.work.infra.Web.ReverseProxy;

public static class ReverseProxyServiceCollectionExtensions
{
    public static IServiceCollection AddSolidCodeReverseProxy(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName)
    {
        services
            .AddReverseProxy()
            .LoadFromConfig(configuration.GetSection(sectionName));

        return services;
    }
}
