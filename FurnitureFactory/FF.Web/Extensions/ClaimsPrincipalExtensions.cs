using System.Security.Claims;

namespace FF.Web.Extensions
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
    }
}
