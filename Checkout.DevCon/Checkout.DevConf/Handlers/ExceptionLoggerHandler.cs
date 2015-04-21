using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace Checkout.DevCon.Handlers
{
    public class ExceptionLoggerHandler : ExceptionLogger
    {
        public override Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            //Nlog handler
            return base.LogAsync(context, cancellationToken);
        }
    }
}