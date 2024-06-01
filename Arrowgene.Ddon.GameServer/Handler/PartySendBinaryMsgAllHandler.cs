using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartySendBinaryMsgAllHandler : GameStructurePacketHandler<C2SPartySendBinaryMsgAllNtc>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartySendBinaryMsgAllHandler));

        public PartySendBinaryMsgAllHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPartySendBinaryMsgAllNtc> packet)
        {
            S2CPartyRecvBinaryMsgAllNtc binaryMsgNtc = new S2CPartyRecvBinaryMsgAllNtc();
            binaryMsgNtc.CharacterId = client.Character.CharacterId;
            binaryMsgNtc.Data = packet.Structure.Data;
            binaryMsgNtc.OnlineStatus = client.Character.OnlineStatus;
            client.Party.SendToAll(binaryMsgNtc);
        }
    }
}
