using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Solidcode.Work.Infra.Validation.Behavior;

namespace Solidcode.Work.Infra.Validation.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers FluentValidation validators from the specified assemblies
    /// and adds the MediatR validation pipeline behavior.
    /// </summary>
    public static IServiceCollection AddSolidCodeValidation(
        this IServiceCollection services,
        params System.Reflection.Assembly[] assemblies)
    {
        if (assemblies is null || assemblies.Length == 0)
            throw new ArgumentException(
                "At least one assembly must be provided.",
                nameof(assemblies));

        services.AddValidatorsFromAssemblies(assemblies);

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        return services;
    }
}