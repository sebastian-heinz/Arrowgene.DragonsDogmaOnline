using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftGetCraftProductHandler : GameRequestPacketHandler<C2SCraftGetCraftProductReq, S2CCraftGetCraftProductRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftGetCraftProductHandler));

        public CraftGetCraftProductHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCraftGetCraftProductRes Handle(GameClient client, C2SCraftGetCraftProductReq request)
        {
            CraftProgress craftProgress = Server.Database.SelectPawnCraftProgress(client.Character.CharacterId, request.CraftMainPawnID);

            S2CCraftGetCraftProductRes craftGetCraftProductRes = new S2CCraftGetCraftProductRes();

            craftGetCraftProductRes.CraftProduct = new CDataCraftProduct()
            {
                ItemID = craftProgress.ItemId,
                ItemNum = craftProgress.CreateCount,
                PlusValue = (byte)craftProgress.PlusValue
            };

            List<CDataItemUpdateResult> itemUpdateResult = Server.ItemManager.AddItem(Server, client.Character, request.StorageType != StorageType.ReceiveInStorageCraft,
                craftProgress.ItemId, craftProgress.CreateCount, (byte)craftProgress.PlusValue);
            craftGetCraftProductRes.UpdateItemList.AddRange(itemUpdateResult);
            
            Server.Database.DeletePawnCraftProgress(client.Character.CharacterId, request.CraftMainPawnID);

            client.Character.Pawns.First(p => p.PawnId == request.CraftMainPawnID).PawnState = PawnState.None;
            Pawn supportPawn1 = client.Character.Pawns.FirstOrDefault(p => p.PawnId == craftProgress.CraftSupportPawnId1, null);
            if (supportPawn1 != null)
            {
                supportPawn1.PawnState = PawnState.None;
            }
            Pawn supportPawn2 = client.Character.Pawns.FirstOrDefault(p => p.PawnId == craftProgress.CraftSupportPawnId2, null);
            if (supportPawn2 != null)
            {
                supportPawn2.PawnState = PawnState.None;
            }
            Pawn supportPawn3 = client.Character.Pawns.FirstOrDefault(p => p.PawnId == craftProgress.CraftSupportPawnId3, null);
            if (supportPawn3 != null)
            {
                supportPawn3.PawnState = PawnState.None;
            }

            return craftGetCraftProductRes;
        }
    }
}
