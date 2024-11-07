using Microsoft.Extensions.DependencyInjection;
using CFA.Business.Services.Implementation;
using CFA.Business.Services.Interfaces;

namespace CFA.Business.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<ICatalogoService, CatalogoService>();
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<IDireccionService, DireccionService>();
        services.AddScoped<ITelefonoService, TelefonoService>();

        return services;
    }
}