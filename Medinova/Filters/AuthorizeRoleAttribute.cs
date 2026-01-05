using System;
using System.Web;
using System.Web.Mvc;

namespace Medinova.Filters
{
    public class AuthorizeRoleAttribute : AuthorizeAttribute
    {
        private readonly string[] _roles;

        public AuthorizeRoleAttribute(params string[] roles)
        {
            _roles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            var role = httpContext.Session["Role"]?.ToString();

            if (string.IsNullOrEmpty(role))
                return false;

            return Array.Exists(_roles, r => r.Equals(role, StringComparison.OrdinalIgnoreCase));
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("/Account/Login");
                return;
            }

            filterContext.Result = new RedirectResult("/Default/Index");
        }
    }
}