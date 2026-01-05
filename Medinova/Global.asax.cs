using Autofac;
using Autofac.Integration.Mvc;
using Medinova.Models;
using Medinova.Repositories.GenericRepositories;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Medinova
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
      
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());      
            builder.RegisterType<MedinovaContext>()
                   .AsSelf()
                   .InstancePerRequest();
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerRequest();
            var container = builder.Build();
            DependencyResolver.SetResolver(
                new AutofacDependencyResolver(container)
            );

            GlobalFilters.Filters.Add(new AuthorizeAttribute());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
