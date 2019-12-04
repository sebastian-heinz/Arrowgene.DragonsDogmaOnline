namespace Ddo.Server.Common.Middleware
{
    public interface IMiddleware<T, TReq, TRes>
    {
        void Handle(T user, TReq request, TRes response, MiddlewareDelegate<T, TReq, TRes> next);
    }
}
