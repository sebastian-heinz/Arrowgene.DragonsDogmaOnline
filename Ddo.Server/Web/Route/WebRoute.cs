using System.Threading.Tasks;
using Arrowgene.Services.Logging;

namespace Ddo.Server.Web.Route
{
    /// <summary>
    /// Implementation of Kestrel server as backend
    /// </summary>
    public abstract class WebRoute : IWebRoute
    {
        protected ILogger Logger => LogProvider.Instance.GetLogger(this);

        public abstract string Route { get; }

        public virtual Task<WebResponse> Get(WebRequest request)
        {
            return WebResponse.NotFound();
        }

        public virtual Task<WebResponse> Post(WebRequest request)
        {
            return WebResponse.NotFound();
        }

        public virtual Task<WebResponse> Put(WebRequest request)
        {
            return WebResponse.NotFound();
        }

        public virtual Task<WebResponse> Delete(WebRequest request)
        {
            return WebResponse.NotFound();
        }
    }
}
