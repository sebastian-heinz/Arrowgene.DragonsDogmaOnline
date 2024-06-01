using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartySendBinaryMsgHandler : GameStructurePacketHandler<C2SPartySendBinaryMsgNtc>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartySendBinaryMsgHandler));

        public PartySendBinaryMsgHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPartySendBinaryMsgNtc> packet)
        {
            if (client.Party != null)
            {
                S2CPartyRecvBinaryMsgNtc binaryMsgNtc = new S2CPartyRecvBinaryMsgNtc();
                binaryMsgNtc.CharacterId = client.Character.CharacterId;
                binaryMsgNtc.Data = packet.Structure.Data;
                binaryMsgNtc.OnlineStatus = client.Character.OnlineStatus;

                client.Party.SendToAllExcept(binaryMsgNtc, client);
            }
        }
    }
}
