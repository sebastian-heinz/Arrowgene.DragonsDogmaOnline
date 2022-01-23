using Arrowgene.Buffers;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ActionSetPlayerActionHistoryHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ActionSetPlayerActionHistoryHandler));

        public ActionSetPlayerActionHistoryHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_ACTION_SET_PLAYER_ACTION_HISTORY_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            // Read request
            C2SActionSetPlayerActionHistoryReq req = EntitySerializer.Get<C2SActionSetPlayerActionHistoryReq>().Read(packet.AsBuffer());

            // Write response
            S2CActionSetPlayerActionHistoryRes res = new S2CActionSetPlayerActionHistoryRes();

            IBuffer resBuffer = new StreamBuffer();
            resBuffer.WriteInt32(0, Endianness.Big); // error
            resBuffer.WriteInt32(0, Endianness.Big); // result

            EntitySerializer.Get<S2CActionSetPlayerActionHistoryRes>().Write(resBuffer, res);
            Packet resPacket = new Packet(PacketId.S2C_ACTION_SET_PLAYER_ACTION_HISTORY_RES, resBuffer.GetAllBytes());
            client.Send(resPacket);
        }

    }
}