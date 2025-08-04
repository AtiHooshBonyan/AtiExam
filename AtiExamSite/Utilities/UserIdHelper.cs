using System.Security.Claims;

namespace AtiExamSite.Utilities
{
    public static class UserIdHelper
    {
        #region [- GetCurrentUserId() -]
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
        #endregion
    }
}
