using System.IO;
using System.Threading.Tasks;
using Ddo.Server.Web;
using Ddo.Server.Web.Middleware;
using Microsoft.Extensions.FileProviders;

namespace Ddo.Server.WebMiddlewares
{
    public class StaticFileMiddleware : IWebMiddleware
    {
        private IFileProvider _provider;

        public StaticFileMiddleware(IFileProvider provider)
        {
            _provider = provider;
        }

        public async Task<WebResponse> Handle(WebRequest request, WebMiddlewareDelegate next)
        {
            WebResponse response = await next(request);
            if (!response.RouteFound && !string.IsNullOrEmpty(request.Path))
            {
                IFileInfo file = _provider.GetFileInfo(request.Path);
                if (file.Exists)
                {
                    response.RouteFound = true;
                    response = new WebResponse();
                    response.StatusCode = 200;
                    string mimeType = MimeTypeMap.GetMimeType(Path.GetExtension(file.Name));
                    response.Header.Add("content-type", mimeType);
                    await response.WriteAsync(file);
                }
            }

            return response;
        }
    }
}
