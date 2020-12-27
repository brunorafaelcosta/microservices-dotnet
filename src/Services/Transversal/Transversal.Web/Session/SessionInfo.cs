using Microsoft.AspNetCore.Http;
using System;
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

                var claimValue = GetClaimValue(Identity.ClaimsConstants.UserIdClaimType);
                if (long.TryParse(claimValue, out long claimParsedValue))
                {
                    userId = claimParsedValue;
                }

                return userId;
            }
        }

        public int? TenantId
        {
            get
            {
                int? tenantId = null;

                var claimValue = GetClaimValue(Identity.ClaimsConstants.TenantIdClaimType);
                if (int.TryParse(claimValue, out int claimParsedValue))
                {
                    tenantId = claimParsedValue;
                }

                return tenantId;
            }
        }

        public string Name
        {
            get
            {
                string name = null;

                var claimValue = GetClaimValue(Identity.ClaimsConstants.NameClaimType);
                if (!string.IsNullOrEmpty(claimValue))
                {
                    name = claimValue;
                }

                return name;
            }
        }

        public string LanguageCode
        {
            get
            {
                string languageCode = null;

                var claimValue = GetClaimValue(Identity.ClaimsConstants.LanguageCodeClaimType);
                if (!string.IsNullOrEmpty(claimValue))
                {
                    languageCode = claimValue;
                }

                return languageCode;
            }
        }

        public DateTime? StartTime => null;

        protected string GetClaimValue(string claimType)
        {
            string value = null;

            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                var user = _httpContextAccessor.HttpContext.User;
                if (user != null && user.HasClaim(c => c.Type == claimType))
                {
                    value = user.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
                }
            }

            return value;
        }
    }
}
