using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftTimeSaveHandler : GameStructurePacketHandler<C2SCraftTimeSaveReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftCancelCraftHandler));

        public CraftTimeSaveHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCraftTimeSaveReq> packet)
        {
            // TODO: consume GG
            
            CraftProgress craftProgress = Server.Database.SelectPawnCraftProgress(client.Character.CharacterId, packet.Structure.PawnID);

            uint remainTime = 0;

            Server.Database.UpdatePawnCraftProgress(client.Character.CharacterId, packet.Structure.PawnID, craftProgress.CraftSupportPawnId1, craftProgress.CraftSupportPawnId2,
                craftProgress.CraftSupportPawnId3, craftProgress.RecipeId, craftProgress.Exp, (int)NpcActionType.NpcActionStithy, craftProgress.ItemId, craftProgress.Unk0,
                remainTime, craftProgress.ExpBonus, craftProgress.CreateCount);

            client.Send(new S2CCraftTimeSaveRes { PawnID = packet.Structure.PawnID, RemainTime = remainTime });
            client.Send(new S2CCraftFinishCraftNtc { PawnId = packet.Structure.PawnID });
        }
    }
}
