using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterPawnDeadHandler : GameStructurePacketHandler<C2SCharacterPawnDeadNtc>
    {
        public CharacterPawnDeadHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCharacterPawnDeadNtc> packet)
        {
            //Unsure what CAPCOM wanted with this packet.
        }
    }
}
