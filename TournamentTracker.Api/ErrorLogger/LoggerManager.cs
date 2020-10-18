using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using NLog;

namespace TournamentTracker.Api.ErrorLogger
{
    public class LoggerManager : ILoggerManager
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public void LogInfo(string message, HttpRequestMessage request)
        {
            logger.Info(message);
        }

        public void LogWarn(string message, HttpRequestMessage request)
        {
            logger.Warn(message);
        }

        public void LogDebug(string message, HttpRequestMessage request)
        {
            logger.Debug(message);
        }

        public void LogError(string message, HttpRequestMessage request)
        {
            logger.Error(message);
        }


        public void LogError(ExceptionLoggerContext context)
        {
            logger.Error(context.Exception, RequestToString(context.Request));
        }

        public async Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            LogError(context);
            await Task.CompletedTask;
        }

        private static string RequestToString(HttpRequestMessage request)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{request.Method} {request.RequestUri} HTTP/{request.Version}");
            stringBuilder.Append(request.Headers);
            stringBuilder.Append(request.Content?.Headers);
            stringBuilder.AppendLine();
            stringBuilder.Append(MappedDiagnosticsLogicalContext.Get("HttpData"));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();
            return stringBuilder.ToString();
        }
    }
}
