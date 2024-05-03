#nullable enable

using System;
using System.Threading.Tasks;
using Arrowgene.Ddon.Rpc;
using Arrowgene.Ddon.Rpc.Web;
using Arrowgene.WebServer;

public class InterceptedRpcWebRoute : RpcWebRoute
{
    private readonly IInterceptor[] _interceptors;
    private readonly RpcWebRoute _interceptedWebRoute;

    public override string Route => this._interceptedWebRoute.Route;

    public InterceptedRpcWebRoute(IRpcExecuter executer, RpcWebRoute interceptedWebRoute, params IInterceptor[] interceptors) : base(executer)
    {
        _interceptedWebRoute = interceptedWebRoute;
        _interceptors = interceptors;
    }

    public override async Task<WebResponse> Get(WebRequest request)
    {
        return await InterceptMethod(request, _interceptedWebRoute.Get);
    }

    public override async Task<WebResponse> Post(WebRequest request)
    {
        return await InterceptMethod(request, _interceptedWebRoute.Post);
    }

    public override async Task<WebResponse> Put(WebRequest request)
    {
        return await InterceptMethod(request, _interceptedWebRoute.Put);
    }

    public override async Task<WebResponse> Delete(WebRequest request)
    {
        return await InterceptMethod(request, _interceptedWebRoute.Delete);
    }

    public override async Task<WebResponse> Head(WebRequest request)
    {
        return await InterceptMethod(request, _interceptedWebRoute.Head);
    }

    private async Task<WebResponse> InterceptMethod(WebRequest request, Func<WebRequest, Task<WebResponse>> afterIntercept)
    {
        foreach (IInterceptor interceptor in _interceptors)
        {
            WebResponse? response = await interceptor.InterceptRequest(request);
            if (response != null)
            {
                return response;
            }
        }
        return await afterIntercept.Invoke(request);
    }
}