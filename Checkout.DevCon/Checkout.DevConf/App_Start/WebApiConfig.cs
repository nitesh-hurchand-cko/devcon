using Checkout.DevCon.Handlers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;

namespace Checkout.DevCon
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.Services.Add(typeof(IExceptionLogger), new Handlers.ExceptionLoggerHandler());
            config.Services.Replace(typeof(IExceptionHandler), new Checkout.DevCon.Handlers.ExceptionHandler());
            config.MessageHandlers.Add(new InOutHandler());

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            //var kernel = new StandardKernel();
            //kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            //kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            //config.DependencyResolver = new NinjectDependencyResolver(kernel);
        }
    }
}
