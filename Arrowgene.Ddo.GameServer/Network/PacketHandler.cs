using Arrowgene.Buffers;
using Arrowgene.Ddo.Shared;

namespace Arrowgene.Ddo.GameServer.Network
{
    public abstract class PacketHandler : IPacketHandler
    {
        protected PacketHandler(DdoGameServer server)
        {
            Server = server;
            Settings = server.Setting;
        }

        protected IBufferProvider Buffer => Util.Buffer;
        protected DdoGameServer Server { get; }
        protected GameServerSetting Settings { get; }

        public abstract ushort Id { get; }
        public abstract void Handle(Client client, Packet packet);
    }
}
