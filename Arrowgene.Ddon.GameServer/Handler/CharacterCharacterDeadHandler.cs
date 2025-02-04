using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterCharacterDeadHandler : GameStructurePacketHandler<C2SCharacterCharacterDeadNtc>
    {
        public CharacterCharacterDeadHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCharacterCharacterDeadNtc> packet)
        {
            // Unsure what CAPCOM wanted with this packet.
            if (Server.EpitaphRoadManager.TrialInProgress(client.Party))
            {
                Server.EpitaphRoadManager.EvaluateDeath(client.Party);
            }
        }
    }
}
