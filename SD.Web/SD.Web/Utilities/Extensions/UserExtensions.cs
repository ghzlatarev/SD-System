using System;
using System.Security.Claims;

namespace SD.Web.Utilities.Extensions
{
    public static class UserExtensions
    {
        public static string GetId(this ClaimsPrincipal user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier);

            return userId?.Value;
        }
    }
}
