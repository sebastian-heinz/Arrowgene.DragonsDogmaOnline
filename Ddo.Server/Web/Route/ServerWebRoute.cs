namespace Ddo.Server.Web.Route
{
    public abstract class ServerWebRoute : WebRoute
    {
        protected DdoServer Server { get; }

        public ServerWebRoute(DdoServer server)
        {
            Server = server;
        }
    }
}
