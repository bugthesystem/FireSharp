using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.WebApp.App_Start;

namespace FireSharp.WebApp
{
    public static class Bootstrapper
    {
       
        public static void Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var builder = new ContainerBuilder();

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.Register(context => new FirebaseConfig()).As<IFirebaseConfig>().SingleInstance();

            builder.RegisterType<FirebaseClient>().As<IFirebaseClient>().SingleInstance();


            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}