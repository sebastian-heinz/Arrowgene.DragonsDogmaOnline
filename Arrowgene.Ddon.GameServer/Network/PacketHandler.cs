using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared;

namespace Arrowgene.Ddon.GameServer.Network
{
    public abstract class PacketHandler : IPacketHandler
    {
        protected PacketHandler(DdonGameServer server)
        {
            Server = server;
            Settings = server.Setting;
        }

        protected IBufferProvider Buffer => Util.Buffer;
        protected DdonGameServer Server { get; }
        protected GameServerSetting Settings { get; }

        public abstract PacketId Id { get; }
        public abstract void Handle(Client client, Packet packet);
    }
}
