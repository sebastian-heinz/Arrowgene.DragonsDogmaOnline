using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartySendBinaryMsgAllHandler : StructurePacketHandler<GameClient, C2SPartySendBinaryMsgAllNtc>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartySendBinaryMsgAllHandler));

        public PartySendBinaryMsgAllHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPartySendBinaryMsgAllNtc> packet)
        {
            foreach(Client otherClient in client.Party.Members)
            {
                S2CPartyRecvBinaryMsgAllNtc binaryMsgNtc = new S2CPartyRecvBinaryMsgAllNtc();
                binaryMsgNtc.CharacterId = client.Character.Id;
                binaryMsgNtc.Data = packet.Structure.Data;
                binaryMsgNtc.OnlineStatus = client.OnlineStatus;
                otherClient.Send(binaryMsgNtc);
            }
        }
    }
}