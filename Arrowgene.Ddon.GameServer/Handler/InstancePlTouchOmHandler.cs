using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstancePlTouchOmHandler : GameStructurePacketHandler<C2SInstancePlTouchOmNtc>
    {
        public InstancePlTouchOmHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstancePlTouchOmNtc> packet)
        {
            //Unsure what CAPCOM wanted with this packet.
        }
    }
}
