using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ServerGameTimeGetBaseinfoHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ServerGameTimeGetBaseinfoHandler));


        public ServerGameTimeGetBaseinfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_SERVER_GAME_TIME_GET_BASEINFO_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            IBuffer buffer = new StreamBuffer();
            buffer.WriteUInt32(0, Endianness.Big);
            buffer.WriteUInt32(0, Endianness.Big);
            buffer.WriteUInt32(1, Endianness.Big);
            buffer.WriteUInt32(0, Endianness.Big);
           // client.Send(new Packet(PacketId.S2C_SERVER_GAME_TIME_GET_BASEINFO_RES, buffer.GetAllBytes(), PacketSource.Server));
        }
    }
}
