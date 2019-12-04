namespace Ddo.Server.Common.Middleware
{
    public delegate void MiddlewareDelegate<T, TReq, TRes>(T user, TReq request, TRes response);
}
