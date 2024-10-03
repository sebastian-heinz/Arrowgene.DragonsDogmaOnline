using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterPawnDownHandler : GameStructurePacketHandler<C2SCharacterPawnDownNtc>
    {
        public CharacterPawnDownHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCharacterPawnDownNtc> packet)
        {
            //Unsure what CAPCOM wanted with this packet.
        }
    }
}
