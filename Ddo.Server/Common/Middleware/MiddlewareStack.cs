using System;

namespace Ddo.Server.Common.Middleware
{
    /// <summary>
    /// Implementation of a middleware
    /// </summary>
    public class MiddlewareStack<T, TReq, TRes>
    {
        private MiddlewareDelegate<T, TReq, TRes> _middlewareDelegate;

        public MiddlewareStack(MiddlewareDelegate<T, TReq, TRes> kernel)
        {
            _middlewareDelegate = kernel;
        }

        public void Start(T user, TReq request, TRes response)
        {
            _middlewareDelegate(user, request, response);
        }

        public MiddlewareStack<T, TReq, TRes> Use(
            Func<MiddlewareDelegate<T, TReq, TRes>, MiddlewareDelegate<T, TReq, TRes>> middleware)
        {
            _middlewareDelegate = middleware(_middlewareDelegate);
            return this;
        }

        public MiddlewareStack<T, TReq, TRes> Use(IMiddleware<T, TReq, TRes> middleware)
        {
            return Use(next => (user, request, response) => middleware.Handle(user, request, response, next));
        }
    }
}
