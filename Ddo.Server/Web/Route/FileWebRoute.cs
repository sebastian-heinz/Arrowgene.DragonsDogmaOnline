using Microsoft.Extensions.FileProviders;

namespace Ddo.Server.Web.Route
{
    public abstract class FileWebRoute : WebRoute
    {
        protected IFileProvider FileProvider { get; }

        public FileWebRoute(IFileProvider fileProvider)
        {
            FileProvider = fileProvider;
        }
    }
}
