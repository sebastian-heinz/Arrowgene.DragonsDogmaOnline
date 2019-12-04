using Arrowgene.Services.Logging;

namespace Ddo.Server.Common.Middleware
{
    public abstract class Middleware<T, TReq, TRes> : IMiddleware<T, TReq, TRes>
    {
        protected Middleware()
        {
            Logger = LogProvider.Logger(this);
        }

        protected ILogger Logger { get; }

        public abstract void Handle(T client, TReq message, TRes response, MiddlewareDelegate<T, TReq, TRes> next);
    }
}
