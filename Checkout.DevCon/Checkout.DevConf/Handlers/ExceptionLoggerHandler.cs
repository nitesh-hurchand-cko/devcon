using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using NLog;

namespace Checkout.DevCon.Handlers
{
    public class ExceptionLoggerHandler : ExceptionLogger
    {
        public override Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            var task = base.LogAsync(context, cancellationToken);

            if (context.Exception == null)
                return task;

            var logEventInfo = new LogEventInfo(LogLevel.Info, "ErrorLogger", null);
            logEventInfo.Properties.Add("CLASSNAME", 
                (context.Exception.TargetSite != null && context.Exception.TargetSite.DeclaringType != null) 
                ? context.Exception.TargetSite.DeclaringType.FullName : string.Empty);
            logEventInfo.Properties.Add("METHODNAME", context.Exception.TargetSite != null 
                ? context.Exception.TargetSite.Name : string.Empty);
            logEventInfo.Properties.Add("SERVERNAME", Environment.MachineName);
            logEventInfo.Properties.Add("MESSAGE", context.Exception.Message);
            logEventInfo.Properties.Add("STACKTRACE", context.Exception.StackTrace);

            LogManager.GetLogger("ErrorLogger").Log(logEventInfo);

            return task;
        }
    }
}