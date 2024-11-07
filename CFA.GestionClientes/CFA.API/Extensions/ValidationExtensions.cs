using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System.Reflection;

namespace CFA.API.Extensions;

public static class ValidationExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        // Registrar todos los validadores del ensamblado
        var assembly = Assembly.GetExecutingAssembly();
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}