using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

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

                var (specialQueue, isSpecial) = Server.ItemManager.HandleSpecialItem(client, new(), (ItemId)craftProgress.ItemId, craftProgress.CreateCount, SpecialItemMode.OnAcquire, connection);

                if (isSpecial)
                {
                    queue.AddRange(specialQueue);
                }
                else if (craftProgress.AdditionalStatusId != 0 && Server.AssetRepository.CraftAddStatusAsset.AddStatuses.TryGetValue(craftProgress.AdditionalStatusId, out var addStatus))
                {
                    var craftItem = new Item()
                    {
                        ItemId = craftProgress.ItemId,
                        PlusValue = (byte)craftProgress.PlusValue,
                        AddStatusParamList = [
                            new() {
                                EnhanceType = EquipEnhanceType.AdditionalCraftMaterial,
                                EnhanceId = addStatus.BuffId,
                            }
                        ]
                    };

                    // AddNewItem doesn't support merging stacks, while AddItem does.
                    // This is split to maintain support for crafting consumables while still allowing us to create a custom item. 
                    craftGetCraftProductRes.UpdateItemList.Add(Server.ItemManager.AddNewItem(
                        Server,
                        client.Character,
                        request.StorageType != StorageType.ReceiveInStorageCraft,
                        craftItem, craftProgress.CreateCount,
                        connection)
                    );
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

                var itemInfo = Server.AssetRepository.ClientItemInfos[craftProgress.ItemId];
                queue.AddRange(Server.AchievementManager.HandleCraft(client, itemInfo, connection));
            });

            queue.Send();
            return craftGetCraftProductRes;
        }
    }
}
