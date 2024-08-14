using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EventEndHandler : GameStructurePacketHandler<C2SEventEndNtc>
    {
        public EventEndHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SEventEndNtc> packet)
        {
            //Unsure what CAPCOM wanted with this packet.
        }
    }
}
