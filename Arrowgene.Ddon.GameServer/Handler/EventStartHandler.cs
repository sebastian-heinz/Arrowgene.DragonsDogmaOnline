using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EventStartHandler : GameStructurePacketHandler<C2SEventStartNtc>
    {
        public EventStartHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SEventStartNtc> packet)
        {
            //Unsure what CAPCOM wanted with this packet.
        }
    }
}
