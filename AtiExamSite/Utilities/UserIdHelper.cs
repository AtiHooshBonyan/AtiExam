using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace AtiExamSite.Utilities
{
    public static class UserIdHelper
    {
        public static Guid? GetCurrentUserId(HttpContext httpContext)
        {
            if (httpContext.User?.Identity?.IsAuthenticated != true)
                return null;

            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return null;

            if (Guid.TryParse(userIdClaim.Value, out var userId))
                return userId;

            return null;
        }
    }
}
