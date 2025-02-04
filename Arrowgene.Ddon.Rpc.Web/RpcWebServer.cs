using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.Rpc.Web.Middleware;
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

            InfoRoute infoRoute = new InfoRoute(this);
            _webServer.AddRoute(infoRoute);

            ChatRoute chatRoute = new ChatRoute(this);
            _webServer.AddRoute(chatRoute);

            AuthMiddleware authMiddleware = new AuthMiddleware(_gameServer.Database);
            authMiddleware.Require(AccountStateType.GameMaster, chatRoute.Route);
            authMiddleware.Require(AccountStateType.GameMaster, infoRoute.Route);
            _webServer.AddMiddleware(authMiddleware);

            #region Internal RPC
            InternalMiddleware internalMiddleware = new InternalMiddleware(_gameServer);

            Route.Internal.CommandRoute trackingRoute = new(this);
            internalMiddleware.Require(trackingRoute.Route);
            _webServer.AddRoute(trackingRoute);

            Route.Internal.InternalChatRoute internalChatRoute = new(this);
            internalMiddleware.Require(internalChatRoute.Route);
            _webServer.AddRoute(internalChatRoute);

            Route.Internal.PacketRoute packetRoute = new(this);
            internalMiddleware.Require(packetRoute.Route);
            _webServer.AddRoute(packetRoute);

            _webServer.AddMiddleware(internalMiddleware);
            #endregion 
        }
    }
}
