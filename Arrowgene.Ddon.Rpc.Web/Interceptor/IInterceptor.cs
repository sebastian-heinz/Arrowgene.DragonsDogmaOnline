#nullable enable

using System.Threading.Tasks;
using Arrowgene.WebServer;

public interface IInterceptor
{
    Task<WebResponse?> InterceptRequest(WebRequest request);
}