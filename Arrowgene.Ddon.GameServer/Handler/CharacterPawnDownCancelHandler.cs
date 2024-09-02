using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterPawnDownCancelHandler : GameStructurePacketHandler<C2SCharacterPawnDownCancelNtc>
    {
        public CharacterPawnDownCancelHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCharacterPawnDownCancelNtc> packet)
        {
            //Unsure what CAPCOM wanted with this packet.
        }
    }
}
