using FluentValidation;
using LibraryOpitech.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryOpitech.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(DependencyInjection).Assembly;
        var handlers = applicationAssembly
            .GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false } && type.Name.EndsWith("Handler", StringComparison.Ordinal));

        foreach (var handler in handlers)
            services.AddScoped(handler);

        services.AddValidatorsFromAssembly(applicationAssembly);

        return services;
    }
}
