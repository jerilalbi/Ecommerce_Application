using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce_Application.Filters
{
    public class JWTAuthorizeAttribute : AuthorizeAttribute
    {
        public string Role { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var token = httpContext.Request.Cookies["Token"]?.Value;
            if (string.IsNullOrEmpty(token)) {
                return false;
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                if (jwtToken.ValidTo < DateTime.UtcNow) { 
                    return false;
                }

                if (!string.IsNullOrEmpty(Role))
                {

                    var allowedRoles = Role.Split(',').Select(r => r.Trim()).ToList();
                    var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
                    if (roleClaim == null || !allowedRoles.Contains(roleClaim.Value,StringComparer.OrdinalIgnoreCase) )
                    {
                        return false;
                    }
                }

                return true;
            }catch { 
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/User/Login");
        }
    }
}