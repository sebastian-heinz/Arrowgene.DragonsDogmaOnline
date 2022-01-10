using Arrowgene.Buffers;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpGetFavoriteWarpPointListHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(WarpGetFavoriteWarpPointListHandler));


        public WarpGetFavoriteWarpPointListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_QUEST_GET_CYCLE_CONTENTS_STATE_LIST_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            IBuffer buffer = new StreamBuffer();
            buffer.WriteUInt32(0, Endianness.Big);
            buffer.WriteUInt32(0, Endianness.Big);
            buffer.WriteBytes(new byte[15]);
          //  client.Send(new Packet(PacketId.S2C_QUEST_GET_CYCLE_CONTENTS_STATE_LIST_RES, buffer.GetAllBytes(), PacketSource.Server));
        }
    }
}
