namespace Nke.Sso.Authorization.Middleware;

public sealed class PermissionAuthorizationHandler(
    IHttpContextAccessor httpContextAccessor,
    IAuthenticater authenticater,
    IAuthorizer authorizer,
    ILogger<PermissionAuthorizationHandler> logger) : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var authorizationHeader = httpContextAccessor.HttpContext?.Request.Headers.Authorization;
        if (string.IsNullOrWhiteSpace(authorizationHeader))
        {
            logger.LogWarning("Authentication failed, no Authorization headers.");
            context.Fail();
            return;
        }

        var machineNameHeader = httpContextAccessor.HttpContext?.Request.Headers["MachineName"];
        if (string.IsNullOrWhiteSpace(machineNameHeader))
        {
            logger.LogWarning("Authentication failed, no MachineName headers.");
            context.Fail();
            return;
        }

        var username = await authenticater.LoginWithTokenAsync(new() { Token = authorizationHeader!, MachineName = machineNameHeader! });
        if (string.IsNullOrWhiteSpace(username))
        {
            logger.LogWarning($"Authentication failed, login with token failed. machine: {machineNameHeader}");
            context.Fail();
            return;
        }

        if (!await authorizer.AuthorizeAsync(new() { Username = username, PermissionName = requirement.PermissionName }))
        {
            logger.LogWarning($"Authorization failed, no match of username and permission. machine: {machineNameHeader}, username: {username}, permission: {requirement.PermissionName}");
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }
}