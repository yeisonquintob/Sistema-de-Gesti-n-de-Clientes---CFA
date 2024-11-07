using Microsoft.OpenApi.Models;
using System.Reflection;
using FluentValidation.AspNetCore;
using CFA.Business.Services.Implementation;
using CFA.Business.Services.Interfaces;
using CFA.Business.Validators;

namespace CFA.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "CFA API",
                Version = "v1",
                Description = "API para la gestión de clientes de la Cooperativa Financiera de Antioquia",
                Contact = new OpenApiContact
                {
                    Name = "Equipo de Desarrollo CFA",
                    Email = "desarrollo@cfa.com.co"
                }
            });

            // Incluir comentarios XML para Swagger
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        return services;
    }

    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("DefaultPolicy",
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Registrar servicios
        services.AddScoped<ICatalogoService, CatalogoService>();
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<IDireccionService, DireccionService>();
        services.AddScoped<ITelefonoService, TelefonoService>();

        return services;
    }

    public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName != null && a.FullName.Contains("CFA")));
        });

        return services;
    }

    public static IServiceCollection AddValidationConfiguration(this IServiceCollection services)
    {
        services.AddFluentValidation(fv =>
        {
            fv.RegisterValidatorsFromAssemblyContaining<ClienteCreateDtoValidator>();
            fv.AutomaticValidationEnabled = false;
        });

        return services;
    }
}