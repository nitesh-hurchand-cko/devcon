using System.Net.Http;
using System.Web;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace Checkout.DevCon.Formatters
{
    public class CustomJsonMediaTypeFormatter : JsonMediaTypeFormatter
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private const string JsonpCallParameter = "jsonp";
        private const string JsonpCallbackString = "callback";

        public CustomJsonMediaTypeFormatter(JsonSerializerSettings jsonSerializerSettings)
        {
            _jsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings();
            SupportedMediaTypes.Add(DefaultMediaType);
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream stream, HttpContent content, IFormatterLogger formatterLogger)
        {
            // Create task reading the content
            return Task.Factory.StartNew(() => JsonConvert.DeserializeObject(content.ReadAsStringAsync().Result, type, _jsonSerializerSettings));
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream stream, HttpContent content, TransportContext transportContext)
        {
            string callback;
            var jsonContent = IsJsonpRequest(out callback) 
                ? string.Format("{0}({1})", (callback ?? JsonpCallbackString), JsonConvert.SerializeObject(value, Formatting.None, _jsonSerializerSettings)) 
                : JsonConvert.SerializeObject(value, Formatting.None, _jsonSerializerSettings);

            var buf = System.Text.Encoding.UTF8.GetBytes(jsonContent);
            stream.Write(buf, 0, buf.Length);

            return Task.FromResult<object>(null);
        }

        private static bool IsJsonpRequest(out string callback)
        {
            callback = null;

            if (HttpContext.Current.Request.HttpMethod != HttpMethod.Get.Method)
            { return false; }

            var queryString = HttpContext.Current.Request.QueryString;

            callback = queryString[JsonpCallbackString];

            return queryString[JsonpCallParameter] != null;
        }
    }
}