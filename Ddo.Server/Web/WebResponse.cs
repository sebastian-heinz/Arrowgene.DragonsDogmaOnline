using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;

namespace Ddo.Server.Web
{
    public class WebResponse
    {
        public static async Task<WebResponse> NotFound()
        {
            WebResponse notFound = new WebResponse();
            notFound.StatusCode = 404;
            await notFound.WriteAsync("404 - route not found");
            return notFound;
        }

        public static async Task<WebResponse> InternalServerError()
        {
            WebResponse internalServerError = new WebResponse();
            internalServerError.StatusCode = 500;
            await internalServerError.WriteAsync("500 - an internal error occured");
            return internalServerError;
        }

        public Stream Body { get; set; }
        public int StatusCode { get; set; }
        public bool RouteFound { get; set; }

        public WebCollection<string, string> Header { get; }

        public WebResponse()
        {
            Body = new MemoryStream();
            Header = new WebCollection<string, string>();
            RouteFound = false;
        }

        public Task WriteAsync(IFileInfo fileInfo, bool contentLength = true)
        {
            if (contentLength)
            {
                Header.Add("content-length", $"{fileInfo.Length}");
            }

            return fileInfo.CreateReadStream().CopyToAsync(Body);
        }

        public Task WriteAsync(string text, bool contentLength = true)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            return WriteAsync(text, Encoding.UTF8, contentLength);
        }

        public Task WriteAsync(string text, Encoding encoding, bool contentLength = true)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));
            byte[] bytes = encoding.GetBytes(text);
            if (contentLength)
            {
                Header.Add("content-length", $"{bytes.Length}");
            }

            return Body.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}
