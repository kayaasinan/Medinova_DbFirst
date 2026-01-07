using Medinova.Services;
using System.Web.Mvc;

namespace Medinova.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
     
            if (filterContext.ExceptionHandled)
                return;

            var controller = filterContext.RouteData.Values["controller"]?.ToString();
            var action = filterContext.RouteData.Values["action"]?.ToString();

            int? userId = null;
            string userName = null;
            string role = null;

            if (filterContext.HttpContext.Session != null)
            {
                userId = filterContext.HttpContext.Session["UserId"] as int?;
                userName = filterContext.HttpContext.Session["UserName"]?.ToString();
                role = filterContext.HttpContext.Session["Role"]?.ToString();
            }

    
            LogService.Error(
                filterContext.Exception,
                action,
                controller,
                userId
            );

     
            filterContext.ExceptionHandled = true;
       
            filterContext.Result = new RedirectResult("/Error/Index");
        }
    }
}