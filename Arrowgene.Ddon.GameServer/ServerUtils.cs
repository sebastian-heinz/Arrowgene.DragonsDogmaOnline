namespace Arrowgene.Ddon.GameServer
{
    public class ServerUtils
    {
        public static bool IsHeadServer(DdonGameServer server)
        {
            return server.RpcManager.HeadServer().Id == server.Id;
        }
    }
}
