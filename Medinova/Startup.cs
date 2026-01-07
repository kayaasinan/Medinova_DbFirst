using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Medinova.Startup))]
namespace Medinova
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
