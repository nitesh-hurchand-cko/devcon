using Checkout.DevCon.Formatters;
using Checkout.DevCon.Handlers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;

namespace Checkout.DevCon
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //ROUTES
            config.MapHttpAttributeRoutes();

            //HANDLERS
            config.Services.Add(typeof(IExceptionLogger), new ExceptionLoggerHandler());
            config.Services.Replace(typeof(IExceptionHandler), new Handlers.ExceptionHandler());
            config.MessageHandlers.Add(new InOutHandler());

            //CORS SUPPORT
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            //FORMATTERS
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

            var xmlSerializerSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateFormatString = "yyyy-MM-ddTHH:mm:ssZ"
            };
            var xmlFormatter = new CustomXmlMediaTypeFormatter(xmlSerializerSettings);
            config.Formatters.Add(xmlFormatter);
        }
    }
}
