using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace Checkout.DevCon.Handlers
{
    public class ExceptionResult : System.Web.Http.Results.ExceptionResult
    {
        public ExceptionResult(Exception exception, bool includeErrorDetail, IContentNegotiator negotiator,
            HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
            : base(exception, includeErrorDetail, negotiator, request, formatters)
        {

        }

        /// <summary>
        /// This method handles all the exceptions generated across the system. Exceptions are logged by GlobalExceptionLogger.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            var message = Request.CreateResponse(HttpStatusCode.BadRequest, new
            {
                ErrorCode = 50000,
                ErrorMessage = "Something went wrong with the data you have sent."
            });
            return Task.FromResult(message);
        }
    }
}