using Serilog;
using System;
using System.Web;

namespace Medinova.Services
{
    public static class LogService
    {
        public static void Info(
                                string message,
                                string action,
                                string controller,
                                int? userId = null,
                                string userName = null,
                                string role = null)
        {
            Log.Information("{Message}", new
            {
                message,
                action,
                controller,
                userId,
                userName,
                role,
                ip = HttpContext.Current?.Request?.UserHostAddress
            });
        }

        public static void Error(
            Exception ex,
            string action,
            string controller,
            int? userId = null)
        {
            Log.Error(ex, "Exception", new
            {
                action,
                controller,
                userId,
                ip = HttpContext.Current?.Request?.UserHostAddress
            });
        }
    }
}