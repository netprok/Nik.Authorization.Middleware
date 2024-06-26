﻿namespace Nik.Authorization.Middleware;

public static class ServicesExtensions
{
    public static IServiceCollection AddNikAuthorizationMiddleware(this IServiceCollection services)
    {
        services
            .AddAuthorization()
            .AddHttpContextAccessor()
            .AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>()
            .AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }
}