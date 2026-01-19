using System.Text.Json;
using System.Text.Json.Serialization;
using SolidCode.Work.Infra.Serialization;

namespace solidcode.work.infra;

public static class JsonSerializerOptionsExtensions
{
    public static JsonSerializerOptions AddSolidCodeDefaults(
        this JsonSerializerOptions options,
        bool includeDateTime = false)
    {
        options.PropertyNameCaseInsensitive = true;

        options.Converters.Add(new JsonStringEnumConverter());
        options.Converters.Add(new DateOnlyJsonConverter());

        if (includeDateTime)
            options.Converters.Add(new DateTimeJsonConverter());

        return options;
    }
}
