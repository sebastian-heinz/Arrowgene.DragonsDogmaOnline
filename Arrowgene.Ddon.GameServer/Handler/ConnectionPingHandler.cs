using Arrowgene.Buffers;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ConnectionPingHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ConnectionPingHandler));


        public ConnectionPingHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_CONNECTION_PING_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            IBuffer buffer = new StreamBuffer();
            buffer.WriteUInt32(0, Endianness.Big);
            buffer.WriteUInt32(0, Endianness.Big);
            buffer.WriteBytes(new byte[15]);
            client.Send(new Packet(PacketId.S2C_CONNECTION_PING_RES, buffer.GetAllBytes(), PacketSource.Server));
        }
    }
}
