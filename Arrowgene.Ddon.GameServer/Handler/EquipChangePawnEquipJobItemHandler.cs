using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipChangePawnEquipJobItemHandler : GameStructurePacketHandler<C2SEquipChangePawnEquipJobItemReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipChangePawnEquipJobItemHandler));
        
        private readonly EquipManager _equipManager;

        public EquipChangePawnEquipJobItemHandler(DdonGameServer server) : base(server)
        {
            _equipManager = server.EquipManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SEquipChangePawnEquipJobItemReq> packet)
        {
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).Single();
            _equipManager.EquipJobItem(Server, client, pawn, packet.Structure.ChangeEquipJobItemList);
        }
    }
}