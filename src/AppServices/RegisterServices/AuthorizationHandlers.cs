using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using MyApp.AppServices.WorkEntries.Permissions;
using MyApp.AppServices.Permissions;
using MyApp.AppServices.Permissions.AppClaims;
using System.Diagnostics.CodeAnalysis;

namespace MyApp.AppServices.RegisterServices;

[SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out")]
public static class AuthorizationHandlers
{
    public static void AddAuthorizationHandlers(this IServiceCollection services)
    {
        services.AddAuthorizationPolicies();

        // Resource/operation-based permission handlers, e.g.:
        // var canAssign = await authorization.Succeeded(User, entryView, WorkEntryOperation.EditWorkEntry);

        services.AddSingleton<IAuthorizationHandler, WorkEntryViewRequirements>();

        // Add claims transformations
        services.AddScoped<IClaimsTransformation, AppClaimsTransformation>();
    }
}
