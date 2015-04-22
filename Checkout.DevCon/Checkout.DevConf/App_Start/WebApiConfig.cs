using System;
using System.Web;
using Checkout.DevCon.Formatters;
using Checkout.DevCon.Handlers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Checkout.DevCon
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //// Web API configuration and services
            //// Configure Web API to use only bearer token authentication.
            //config.SuppressDefaultHostAuthentication();

            // Web API routes
            config.MapHttpAttributeRoutes();

            //Handlers
            config.Services.Add(typeof(IExceptionLogger), new Handlers.ExceptionLoggerHandler());
            config.Services.Replace(typeof(IExceptionHandler), new Checkout.DevCon.Handlers.ExceptionHandler());
            config.MessageHandlers.Add(new InOutHandler());

            //Cors
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            //Formatters
            config.Formatters.Clear();
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateFormatString = "yyyy-MM-ddTHH:mm:ssZ"
            };
            var jsonFormatter = new CustomJsonMediaTypeFormatter(jsonSerializerSettings);
            config.Formatters.Add(jsonFormatter);
        }
    }
}
