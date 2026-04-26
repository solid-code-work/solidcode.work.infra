using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using solidcode.work.infra.Serialization;

namespace solidcode.work.infra.DependencyInjection;

public static class JsonSerializerExtensions
{
    public static void AddSolidCodeDefaults(
        this IServiceCollection services)
    {
        services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
            options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
        });
    }
}
