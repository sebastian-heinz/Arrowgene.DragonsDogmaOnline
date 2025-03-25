using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipChangePawnEquipJobItemHandler : GameRequestPacketQueueHandler<C2SEquipChangePawnEquipJobItemReq, S2CEquipChangePawnEquipJobItemRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipChangePawnEquipJobItemHandler));
        
        public EquipChangePawnEquipJobItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SEquipChangePawnEquipJobItemReq request)
        {
            Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);
            return Server.EquipManager.EquipJobItem(Server, client, pawn, request.ChangeEquipJobItemList);
        }
    }
}
