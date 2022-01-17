using Arrowgene.Buffers;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ServerGetRealTimeHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ServerGetRealTimeHandler));


        public ServerGetRealTimeHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_SERVER_GET_REAL_TIME_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            IBuffer buffer = new StreamBuffer();
            buffer.WriteUInt32(0, Endianness.Big);
            buffer.WriteUInt32(0, Endianness.Big);
            buffer.WriteBytes(new byte[15]);
            client.Send(new Packet(PacketId.S2C_SERVER_GET_REAL_TIME_RES, buffer.GetAllBytes()));
        }
    }
}
