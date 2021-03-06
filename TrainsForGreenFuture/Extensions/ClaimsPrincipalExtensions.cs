namespace TrainsForGreenFuture.Extensions
{
    using System.Security.Claims;

    using static Areas.RolesConstants;
    public static class ClaimsPrincipalExtensions
    {
        public static string Id(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.NameIdentifier).Value;

        public static bool IsAdmin(this ClaimsPrincipal user)
            => user.IsInRole(AdministratorRole);
    }
}

