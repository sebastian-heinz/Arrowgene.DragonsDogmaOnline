using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
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
            S2CCraftGetCraftProductRes craftGetCraftProductRes = new S2CCraftGetCraftProductRes();

            PacketQueue queue = new();

            Server.Database.ExecuteInTransaction(connection =>
            {
                CraftProgress craftProgress = Server.Database.SelectPawnCraftProgress(client.Character.CharacterId, request.CraftMainPawnID, connection);

                craftGetCraftProductRes.CraftProduct = new CDataCraftProduct()
                {
                    ItemID = craftProgress.ItemId,
                    ItemNum = craftProgress.CreateCount,
                    PlusValue = (byte)craftProgress.PlusValue
                };

                var (specialQueue, isSpecial) = Server.ItemManager.HandleSpecialItem(client, new(), (ItemId)craftProgress.ItemId, craftProgress.CreateCount, connection);

                if (isSpecial)
                {
                    queue.AddRange(specialQueue);
                }
                else
                {
                    List<CDataItemUpdateResult> itemUpdateResult = Server.ItemManager.AddItem(
                       Server,
                       client.Character,
                       request.StorageType != StorageType.ReceiveInStorageCraft,
                       craftProgress.ItemId,
                       craftProgress.CreateCount,
                       (byte)craftProgress.PlusValue,
                       connection
                    );
                    craftGetCraftProductRes.UpdateItemList.AddRange(itemUpdateResult);
                }

                Server.Database.DeletePawnCraftProgress(client.Character.CharacterId, request.CraftMainPawnID, connection);

                Pawn mainPawn = client.Character.Pawns.First(p => p.PawnId == request.CraftMainPawnID);
                mainPawn.PawnState = PawnState.None;
                Server.Database.UpdatePawnBaseInfo(mainPawn, connection);
                foreach (var supportId in new List<uint>(){ 
                    craftProgress.CraftSupportPawnId1, 
                    craftProgress.CraftSupportPawnId2, 
                    craftProgress.CraftSupportPawnId3 
                })
                {
                    Pawn supportPawn = client.Character.Pawns.FirstOrDefault(p => p.PawnId == supportId, null);
                    if (supportPawn != null)
                    {
                        supportPawn.PawnState = PawnState.None;
                        if (supportPawn.PawnType == PawnType.Main)
                        {
                            Server.Database.UpdatePawnBaseInfo(supportPawn, connection);
                        }
                    }
                }

                var itemInfo = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, craftProgress.ItemId);
                queue.AddRange(Server.AchievementManager.HandleCraft(client, itemInfo, connection));
            });

            queue.Send();
            return craftGetCraftProductRes;
        }
    }
}
