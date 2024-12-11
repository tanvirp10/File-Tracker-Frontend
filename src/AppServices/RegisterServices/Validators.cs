using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace MyApp.AppServices.RegisterServices;

public static class Validators
{
    // Add all validators
    public static IServiceCollection AddValidators(this IServiceCollection services) => 
        services.AddValidatorsFromAssemblyContaining(typeof(Validators));
}
