using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.DevCon.Handlers
{
    public class InOutHandler : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            var assembly = Assembly.GetExecutingAssembly();
            //var productVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            var version = assembly.GetName().Version.ToString();
            //response.Headers.Add("Product", productVersion);
            response.Headers.Add("Version", version);
            return response;
        }
    }
}