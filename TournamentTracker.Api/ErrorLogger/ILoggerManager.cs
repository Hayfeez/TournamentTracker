using System.Net.Http;
using System.Web.Http.ExceptionHandling;

namespace TournamentTracker.Api.ErrorLogger
{
    public interface ILoggerManager : IExceptionLogger
    {
        void LogInfo(string message, HttpRequestMessage request = null);
        void LogWarn(string message, HttpRequestMessage request = null);
        void LogDebug(string message, HttpRequestMessage request = null);
        void LogError(ExceptionLoggerContext context);
        void LogError(string message, HttpRequestMessage request = null);
    }
}
