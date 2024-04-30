namespace NiC.Authorization.Middleware;

public sealed class HasPermissionAttribute(string permission) : AuthorizeAttribute(policy: permission)
{
}