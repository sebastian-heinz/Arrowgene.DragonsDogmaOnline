using System;
using Arrowgene.Services.Logging;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Ddo.Server.Web.Server.Kestrel
{
    public class KestrelLogger : ILogger
    {
        private readonly string _name;
        private Arrowgene.Services.Logging.ILogger _logger;

        public KestrelLogger(string name)
        {
            _name = name;
            _logger = LogProvider.Logger<Logger>(name);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (exception != null)
            {
                _logger.Exception(exception);
                return;
            }

            string message = $"{formatter(state, exception)}";
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                case LogLevel.None:
                case LogLevel.Information:
                    _logger.Debug(message);
                    break;
                case LogLevel.Warning:
                    _logger.Info(message);
                    break;
                case LogLevel.Error:
                case LogLevel.Critical:
                    _logger.Error(message);
                    break;
            }
        }
    }
}
