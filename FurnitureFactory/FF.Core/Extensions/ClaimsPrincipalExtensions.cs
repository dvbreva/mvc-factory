using System;
using System.Security.Claims;

namespace FF.Core.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsAdmin(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.IsInRole("Admin");
        }

        public static bool IsRegularUser(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.IsInRole("Regular");
        }

        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);

            return claim?.Value;
        }
    }
}
