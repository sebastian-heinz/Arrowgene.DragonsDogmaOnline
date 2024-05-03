using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.Rpc.Web.Route;
using Arrowgene.Ddon.WebServer;

namespace Arrowgene.Ddon.Rpc.Web
{
    /// <summary>
    /// HTTP Protocol for RPC calls
    /// </summary>
    public class RpcWebServer : RpcServer
    {
        private readonly DdonWebServer _webServer;

        public RpcWebServer(DdonWebServer webServer, DdonGameServer gameServer) : base(gameServer)
        {
            _webServer = webServer;
        }

        public void Init()
        {
            _webServer.AddRoute(new SpawnRoute(this));
            _webServer.AddRoute(new InfoRoute(this));
            _webServer.AddRoute(new InterceptedRpcWebRoute(this, new ChatRoute(this), new AuthInterceptor(_gameServer.Database, AccountStateType.GameMaster)));
        }
    }
}
