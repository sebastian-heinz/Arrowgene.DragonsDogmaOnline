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
            _webServer.AddRoute(new ServerStatusRoute(this));
            _webServer.AddRoute(new SpawnRoute(this));
            _webServer.AddRoute(new InfoRoute(this));

            ChatRoute chatRoute = new ChatRoute(this);
            _webServer.AddRoute(chatRoute);

            FindPlayerRoute findPlayerRoute = new(this);
            _webServer.AddRoute(findPlayerRoute);

            KickRoute kickRoute = new(this);
            _webServer.AddRoute(kickRoute);

            AuthMiddleware authMiddleware = new AuthMiddleware(_gameServer.Database);
            authMiddleware.Require(AccountStateType.GameMaster, chatRoute.Route);
            authMiddleware.Require(AccountStateType.GameMaster, findPlayerRoute.Route);
            authMiddleware.Require(AccountStateType.GameMaster, kickRoute.Route);
            _webServer.AddMiddleware(authMiddleware);
        }
    }
}
