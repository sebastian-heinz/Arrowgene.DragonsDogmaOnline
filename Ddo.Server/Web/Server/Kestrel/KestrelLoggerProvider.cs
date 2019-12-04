using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Ddo.Server.Web.Server.Kestrel
{
    public class KestrelLoggerProvider: ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, KestrelLogger> _loggers = new ConcurrentDictionary<string, KestrelLogger>();

        public KestrelLoggerProvider()
        {

        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new KestrelLogger(name));
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}
