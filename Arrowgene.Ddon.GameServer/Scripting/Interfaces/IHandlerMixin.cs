using Arrowgene.Ddon.Server.Network;

namespace Arrowgene.Ddon.GameServer.Scripting.Interfaces
{
    public abstract class IHandlerMixin<T,U> 
        where T : new()
        where U : new()
    {
        public virtual U GameRequestPacketHandler(DdonGameServer server, GameClient client, T request)
        {
            return new();
        }

        public virtual PacketQueue GameRequestPacketQueueHandler(DdonGameServer server, GameClient client, T request)
        {
            return new();
        }
    }
}
