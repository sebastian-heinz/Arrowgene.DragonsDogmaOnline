using Arrowgene.Ddon.Server.Network;

namespace Arrowgene.Ddon.GameServer.Scripting.Interfaces
{
    public abstract class IHandlerMixin<T,U>
    {
        public abstract U GameRequestPacketHandler(DdonGameServer server, GameClient client, T request);
        public abstract PacketQueue GameRequestPacketQueueHandler(DdonGameServer server, GameClient client, T request);
    }
}
