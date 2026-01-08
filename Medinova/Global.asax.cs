using Autofac;
using Autofac.Integration.Mvc;
using Medinova.Models;
using Medinova.Repositories.GenericRepositories;
using Medinova.Services;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
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
            builder.RegisterType<AppointmentMlService>().AsSelf().InstancePerRequest();
            builder.RegisterType<DoctorForecastMlService>().AsSelf().InstancePerRequest();
            builder.RegisterType<DepartmentAnalyticsService>().AsSelf().InstancePerRequest();
            builder.RegisterType<DepartmentForecastMlService>().AsSelf().InstancePerRequest();
            builder.RegisterType<MlDashboardCardService>().AsSelf().InstancePerRequest();
     

            var container = builder.Build();
            DependencyResolver.SetResolver(
              new AutofacDependencyResolver(container)
          );
            Log.Logger = new LoggerConfiguration()
                            .Enrich.FromLogContext()
                            .Enrich.WithEnvironmentName()
                            .Enrich.WithThreadId()
                            .WriteTo.Async(a => a.Elasticsearch(
         new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
         {
             IndexFormat = "medinova-logs-{0:yyyy.MM.dd}",
             AutoRegisterTemplate = true,
             MinimumLogEventLevel = Serilog.Events.LogEventLevel.Information
         }))
     .CreateLogger();
            GlobalFilters.Filters.Add(new AuthorizeAttribute());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
