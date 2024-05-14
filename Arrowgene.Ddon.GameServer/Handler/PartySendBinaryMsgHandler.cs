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
            // foreach(GameClient otherClient in Server.Clients.Where(x => packet.Structure.CharacterIdList.Select(x => x.Value).Contains(x.Character.CharacterId)))
            foreach (GameClient otherClient in Server.Clients)
            {
                if (otherClient == null || otherClient.Character == null)
                {
                    continue;
                }

                if (!packet.Structure.CharacterIdList.Select(x => x.Value).Contains(otherClient.Character.CharacterId))
                {
                    continue;
                }

                S2CPartyRecvBinaryMsgNtc binaryMsgNtc = new S2CPartyRecvBinaryMsgNtc();
                binaryMsgNtc.CharacterId = client.Character.CharacterId;
                binaryMsgNtc.Data = packet.Structure.Data;
                binaryMsgNtc.OnlineStatus = client.Character.OnlineStatus;
                otherClient.Send(binaryMsgNtc);
            }
        }
    }
}
