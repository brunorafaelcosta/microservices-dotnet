using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Transversal.Web.Session
{
    public class SessionInfo : Common.Session.ISessionInfo
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionInfo(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long? UserId
        {
            get
            {
                long? userId = null;

                var user = _httpContextAccessor.HttpContext.User;
                if (user != null && user.HasClaim(c => c.Type == Identity.ClaimsConstants.UserIdClaimType))
                {
                    string claimValue = user.Claims.FirstOrDefault(c => c.Type == Identity.ClaimsConstants.UserIdClaimType)?.Value;
                    if (long.TryParse(claimValue, out long claimParsedValue))
                    {
                        userId = claimParsedValue;
                    }
                }

                return userId;
            }
        }

        public int? TenantId
        {
            get
            {
                int? tenantId = null;

                var user = _httpContextAccessor.HttpContext.User;
                if (user != null && user.HasClaim(c => c.Type == Identity.ClaimsConstants.TenantIdClaimType))
                {
                    string claimValue = user.Claims.FirstOrDefault(c => c.Type == Identity.ClaimsConstants.TenantIdClaimType)?.Value;
                    if (int.TryParse(claimValue, out int claimParsedValue))
                    {
                        tenantId = claimParsedValue;
                    }
                }

                return tenantId;
            }
        }
    }
}
