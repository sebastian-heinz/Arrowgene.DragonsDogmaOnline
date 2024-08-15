using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterCharacterDownCancelHandler : GameStructurePacketHandler<C2SCharacterCharacterDownCancelNtc>
    {
        public CharacterCharacterDownCancelHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCharacterCharacterDownCancelNtc> packet)
        {
            //Unsure what CAPCOM wanted with this packet.
        }
    }
}
