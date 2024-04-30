namespace Nke.Sso.Authorization.Middleware;

public static class ServicesExtensions
{
    public static IServiceCollection AddSsoAuthorizationMiddleware(this IServiceCollection services)
    {
        services
            .AddAuthorization()
            .AddHttpContextAccessor()
            .AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>()
            .AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }
}