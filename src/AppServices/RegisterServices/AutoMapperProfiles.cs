using Microsoft.Extensions.DependencyInjection;
using MyApp.AppServices.AutoMapper;

namespace MyApp.AppServices.RegisterServices;

public static class AutoMapperProfiles
{
    // Add AutoMapper profiles
    public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services) => 
        services.AddAutoMapper(configuration => configuration.AddProfile<AutoMapperProfile>());
}
