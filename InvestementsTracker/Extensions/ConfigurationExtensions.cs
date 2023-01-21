using AutoMapper;
using InvestementsTracker.Converters;
using InvestementsTracker.InPzuDatabase;
using InvestementsTracker.Services;
using InvestementsTracker.Services.InPzu;
using Microsoft.EntityFrameworkCore;

namespace InvestementsTracker.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        typeof(Program)
            .Assembly
            .GetTypes()
            .Where(x => x.IsSubclassOf(typeof(Profile)))
            .ToList()
            .ForEach(x => services.AddAutoMapper(x));
        return services;
    }

    public static IServiceCollection AddPgDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<InPzuDataContext>(opts =>
            opts
            .UseNpgsql(configuration.GetConnectionString("pg"), b => b.MigrationsAssembly("InvestementsTracker"))
            .EnableSensitiveDataLogging(configuration.GetValue<bool>("pg:EnableSensitiveDataLogging")));
        return services;
    }

    public static IMvcBuilder AddJsonConverters(this IMvcBuilder builder)
    {
        return builder.AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        });
    }

    public static WebApplicationBuilder AddCommonServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(new DateFormatter());
        builder.Services.AddSingleton(new JokeService());
        return builder;
    }

    public static IServiceCollection AddInPzuServices(this IServiceCollection services)
    {
        services.AddScoped<IInPzuScrapingService, InPzuScrapingService>();
        services.AddScoped<IInPzuRepository, InPzuRepository>();
        return services;
    }

    public static void AddResponseHeaderChanges(this WebApplication app)
    {
        var jokeService = app.Services.GetService<JokeService>();

        app.Use(async (context, next) => {
            context.Response.Headers.Add("joke", jokeService.GetJoke());
            await next().ConfigureAwait(false);
        });
    }
}