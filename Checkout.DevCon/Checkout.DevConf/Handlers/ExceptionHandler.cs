using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace Checkout.DevCon.Handlers
{
    public class ExceptionHandler : System.Web.Http.ExceptionHandling.ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var request = context.RequestContext;
            var config = request.Configuration;
            context.Result = new ExceptionResult(context.Exception, request != null && request.IncludeErrorDetail,
                config.Services.GetContentNegotiator(), context.Request, config.Formatters);
        }

        public override bool ShouldHandle(ExceptionHandlerContext context)
        {
            return true;
        }
    }
}