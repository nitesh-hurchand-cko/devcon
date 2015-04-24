using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Formatting = Newtonsoft.Json.Formatting;

namespace Checkout.DevCon.Formatters
{
    public class CustomXmlMediaTypeFormatter : XmlMediaTypeFormatter
    {
        private readonly JsonSerializerSettings _xmlSerializerSettings;
        private const string JsonpCallParameter = "jsonp";
        private const string JsonpCallbackString = "callback";

        public CustomXmlMediaTypeFormatter(JsonSerializerSettings xmlSerializerSettings)
        {
            _xmlSerializerSettings = xmlSerializerSettings ?? new JsonSerializerSettings();
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
            return Task.Factory.StartNew(() =>
            {
                var xml = new XmlDocument();
                xml.Load(stream);
                var json = JsonConvert.SerializeXmlNode(xml, Formatting.None, true);
                return JsonConvert.DeserializeObject(json, type, _xmlSerializerSettings);
            });
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream stream, HttpContent content, TransportContext transportContext)
        {
            string callback;
            var jsonContent = IsJsonpRequest(out callback)
                ? string.Format("{0}({1})", (callback ?? JsonpCallbackString), JsonConvert.SerializeObject(value, Formatting.None, _xmlSerializerSettings))
                : JsonConvert.SerializeObject(value, Formatting.None, _xmlSerializerSettings);

            var result = JsonConvert.DeserializeXmlNode(jsonContent, "data", true);
            var cleanString = Regex.Replace(result.OuterXml, "xmlns:json=\"http://james.newtonking.com/projects/json\"", string.Empty);
            var buf = System.Text.Encoding.UTF8.GetBytes(cleanString);
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