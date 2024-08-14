using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceCharacterStartBadStatusHandler : GameStructurePacketHandler<C2SInstanceCharacterStartBadStatusNtc>
    {
        public InstanceCharacterStartBadStatusHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceCharacterStartBadStatusNtc> packet)
        {
            //Unsure what CAPCOM wanted with this packet.
        }
    }
}
