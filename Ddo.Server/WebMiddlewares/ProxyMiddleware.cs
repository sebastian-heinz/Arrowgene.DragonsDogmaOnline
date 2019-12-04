using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Ddo.Server.Web;
using Ddo.Server.Web.Middleware;

namespace Ddo.Server.WebMiddlewares
{
    public class ProxyMiddleware : IWebMiddleware
    {
        private readonly HttpClient _client;

        public ProxyMiddleware()
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) => true;
            _client = new HttpClient(handler);
        }

        public async Task<WebResponse> Handle(WebRequest request, WebMiddlewareDelegate next)
        {
            WebResponse response = await next(request);

            if (!response.RouteFound && !string.IsNullOrEmpty(request.Path))
            {
                HttpResponseMessage cRes = null;
                //string url = $"{request.Scheme}://106.185.75.153/{request.Path}{request.QueryString}";
                string url = $"{request.Scheme}://{request.Host}/{request.Path}{request.QueryString}";
                if (request.Method == WebRequestMethod.Get)
                {
                    using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, url))
                    {
                        foreach (var key in request.Header.Keys)
                        {
                            string value = request.Header[key];
                            requestMessage.Headers.TryAddWithoutValidation(key, value);
                        }

                        cRes = await _client.SendAsync(requestMessage);
                    }
                }

                if (cRes != null)
                {
                    response = new WebResponse();
                    response.Body.Position = 0;
                    await cRes.Content.CopyToAsync(response.Body);
                    response.StatusCode = (int) cRes.StatusCode;
                    foreach (KeyValuePair<string, IEnumerable<string>> header in cRes.Headers)
                    {
                        response.Header.Add(header.Key, string.Join(';', header.Value));
                    }
                }
            }

            return response;
        }
    }
}
