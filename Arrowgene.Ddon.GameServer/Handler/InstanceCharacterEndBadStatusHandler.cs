using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceCharacterEndBadStatusHandler : GameStructurePacketHandler<C2SInstanceCharacterEndBadStatusNtc>
    {
        public InstanceCharacterEndBadStatusHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceCharacterEndBadStatusNtc> packet)
        {
            //Unsure what CAPCOM wanted with this packet.
        }
    }
}
