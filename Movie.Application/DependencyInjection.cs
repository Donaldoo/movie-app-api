using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Movie.Application.Account;
using Movie.Application.Common;
using Movie.Application.Common.Behaviours;
using Movie.Application.Common.Dates;
using Movie.Application.Internationalization;
using Movie.Application.PaymentClient;
using Movie.Domain.Common.Attributes;
using SocialMedia.Application.Common.Behaviours;

namespace Movie.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, ISettings settings)
    {
        services.AddLogging();
        services.AddValidation();
        
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(RequestPerformanceBehaviour<,>));
            cfg.AddOpenBehavior(typeof(RequestValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(AuthenticateBehaviour<,>));
        });
        services.AddSingleton<ILanguageResource, EnglishLanguageResource>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<IDateTimeFactory, DateTimeFactory>();
        services.AddScoped<IPaymentClient, PaymentClient.PaymentClient>();
        services.AddScoped<IDateFormatter, DateFormatter>();
        services.AddSingleton(settings);

        return services;
    }
    
    private static IServiceCollection AddValidation(this IServiceCollection services)
    {
        var excludedTypes =
            AttributesUtils.GetTypesWithAttribute(Assembly.GetExecutingAssembly(), typeof(ExcludeFromDependency));

        AssemblyScanner
            .FindValidatorsInAssembly(Assembly.GetExecutingAssembly())
            .ForEach(result =>
            {
                if (!excludedTypes.Contains(result.ValidatorType))
                    services.AddScoped(result.InterfaceType, result.ValidatorType);
            });

        return services;
    }
}