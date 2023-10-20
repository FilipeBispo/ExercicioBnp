using ExercicioBnp.Infrastructure;
using ExercicioBnp.Infrastructure.Interfaces;
using ExercicioBnp.Middleware;
using ExercicioBnp.Services;
using ExercicioBnp.Services.Interfaces;
using ExercicioBnp.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder);
ConfigureLogging(builder);

var app = builder.Build();

ConfigureMiddlewares(app);

app.Run();

void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

    builder.Services.AddTransient<IIsinRepository, IsinRepository>();
    builder.Services.AddTransient<IExternalPriceService, ExternalPriceService>();

    builder.Services.AddHttpClient();

    builder.Services.AddOptions();
    builder.Services.Configure<IsinSettings>(builder.Configuration.GetSection("IsinSettings"));
    builder.Services.Configure<ExternalPriceServiceSettings>(builder.Configuration.GetSection("ExternalPriceServiceSettings"));

}

void ConfigureLogging(WebApplicationBuilder builder)
{
    builder.Logging.AddConsole();
}

void ConfigureMiddlewares(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
        });
    }

    app.UseMiddleware<ExceptionMiddleware>();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
}
app.Run();
