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

                if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
                {
                    var user = _httpContextAccessor.HttpContext.User;
                    if (user != null && user.HasClaim(c => c.Type == Identity.ClaimsConstants.UserIdClaimType))
                    {
                        string claimValue = user.Claims.FirstOrDefault(c => c.Type == Identity.ClaimsConstants.UserIdClaimType)?.Value;
                        if (long.TryParse(claimValue, out long claimParsedValue))
                        {
                            userId = claimParsedValue;
                        }
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

                if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
                {
                    var user = _httpContextAccessor.HttpContext.User;
                    if (user != null && user.HasClaim(c => c.Type == Identity.ClaimsConstants.TenantIdClaimType))
                    {
                        string claimValue = user.Claims.FirstOrDefault(c => c.Type == Identity.ClaimsConstants.TenantIdClaimType)?.Value;
                        if (int.TryParse(claimValue, out int claimParsedValue))
                        {
                            tenantId = claimParsedValue;
                        }
                    }
                }

                return tenantId;
            }
        }

        public string LanguageCode
        {
            get
            {
                string languageCode = null;

                if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
                {
                    var user = _httpContextAccessor.HttpContext.User;
                    if (user != null && user.HasClaim(c => c.Type == Identity.ClaimsConstants.LanguageCodeClaimType))
                    {
                        string claimValue = user.Claims.FirstOrDefault(c => c.Type == Identity.ClaimsConstants.LanguageCodeClaimType)?.Value;
                        if (!string.IsNullOrEmpty(claimValue))
                        {
                            languageCode = claimValue;
                        }
                    }
                }
                

                return languageCode;
            }
        }
    }
}
