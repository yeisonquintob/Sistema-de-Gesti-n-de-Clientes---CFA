using CFA.API.Extensions;
using CFA.API.Middleware;
using CFA.Business.Extensions;
using CFA.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/cfa-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Agregar servicios al contenedor
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Configurar DbContext
builder.Services.AddDbContext<CFAContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        }));

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CFAPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Ajustar según necesidades
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "CFA API - Gestión de Clientes",
        Version = "v1",
        Description = "API para la gestión de clientes de la Cooperativa Financiera de Antioquia",
        Contact = new()
        {
            Name = "Equipo de Desarrollo CFA",
            Email = "desarrollo@cfa.com.co"
        }
    });

    // Incluir comentarios XML si están configurados
    var xmlFile = "CFA.API.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Registrar servicios de la aplicación
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddBusinessServices(); // Extensión de Business
builder.Services.AddValidators();      // Extensión de Business

// Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = Microsoft.AspNetCore.RateLimiting.PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.User?.Identity?.Name ?? context.Request.Headers.Host.ToString(),
            factory: partition => new Microsoft.AspNetCore.RateLimiting.FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }));
});

// Configurar compresión de respuesta
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CFA API V1");
        c.RoutePrefix = string.Empty; // Para servir la UI de Swagger en la raíz
    });
}
else
{
    // Configuraciones de seguridad para producción
    app.UseHsts();
}

// Middleware de manejo de errores personalizado
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseCors("CFAPolicy");
app.UseRateLimiter();

// Headers de seguridad
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers()
   .RequireAuthorization();

// Asegurar que la base de datos está creada y actualizada
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CFAContext>();
    try
    {
        context.Database.Migrate();
        Log.Information("Base de datos actualizada correctamente");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Error al actualizar la base de datos");
        throw;
    }
}

try
{
    Log.Information("Iniciando aplicación CFA");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación CFA se detuvo inesperadamente");
}
finally
{
    Log.CloseAndFlush();
}