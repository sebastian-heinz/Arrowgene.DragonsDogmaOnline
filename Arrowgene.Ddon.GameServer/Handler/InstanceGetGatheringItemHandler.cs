#nullable enable
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.GatheringItems.Generators;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetGatheringItemHandler : GameRequestPacketQueueHandler<C2SInstanceGetGatheringItemReq, S2CInstanceGetGatheringItemRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetGatheringItemHandler));

        public InstanceGetGatheringItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SInstanceGetGatheringItemReq request)
        {
            PacketQueue packetQueue = new();

            S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = request.EquipToCharacter == 0 ? ItemNoticeType.Gather : ItemNoticeType.GatherEquipItem
            };

            uint posId = request.PosId;
            var stageId = request.LayoutId.AsStageLayoutId();
            var (_, itemList) = client.InstanceGatheringItemManager.FetchOrGenerate(stageId, posId);

            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (CDataGatheringItemGetRequest gatheringItemRequest in request.GatheringItemGetRequestList)
                {
                    InstancedGatheringItem gatheredItem = itemList.ElementAtOrDefault((int)gatheringItemRequest.SlotNo)
                        ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_INSTANCE_AREA_INVALID_GATHERING_ITEM_POS_ID,
                        $"Invalid gathering item index at {stageId}.{posId}");
                    
                    packetQueue.AddRange(Server.ItemManager.GatherItem(client, ntc, gatheredItem, gatheringItemRequest.Num, connection));

                    if (HandleOneOffGatherItem(client, stageId, posId, gatheredItem, connection, out var oneOffQueue))
                    {
                        packetQueue.AddRange(oneOffQueue);
                    }
                }
            });

            client.Enqueue(ntc, packetQueue);

            S2CInstanceGetGatheringItemRes res = new S2CInstanceGetGatheringItemRes();
            res.LayoutId = request.LayoutId;
            res.PosId = request.PosId;
            res.GatheringItemGetRequestList = request.GatheringItemGetRequestList;
            client.Enqueue(res, packetQueue);

            if (request.EquipToCharacter == 1)
            {
                var itemInfo = Server.AssetRepository.ClientItemInfos[ntc.UpdateItemList[0].ItemList.ItemId];
                var equipInfo = new CDataCharacterEquipInfo()
                {
                    EquipItemUId = ntc.UpdateItemList[0].ItemList.ItemUId,
                    EquipCategory = (byte)itemInfo.EquipSlot,
                    EquipType = EquipType.Performance,
                };

                Server.Database.ExecuteInTransaction(connection =>
                {
                    packetQueue.AddRange(Server.EquipManager.HandleChangeEquipList(
                        Server,
                        client,
                        client.Character,
                        new List<CDataCharacterEquipInfo>() { equipInfo },
                        ItemNoticeType.GatherEquipItem,
                        new List<StorageType>() { StorageType.ItemBagEquipment },
                        connection));
                });
            }

            return packetQueue;
        }

        // TODO: Expose these amounts to settings
        private static readonly uint ONEOFF_GATHER_PP_AMOUNT = 5;
        private static readonly uint ONEOFF_GATHER_EXP_AMOUNT = 50000;

        private bool HandleOneOffGatherItem(
            GameClient client, 
            StageLayoutId stageLayoutId, 
            uint index, 
            InstancedGatheringItem gatheredItem,
            DbConnection? connectionIn,
            out PacketQueue queue)
        {
            queue = new();
            StageInfo stageInfo = Stage.StageInfoFromStageLayoutId(stageLayoutId);
            if (OneOffGatheringItemGenerator.ValidStages.TryGetValue(stageInfo, out ItemId oneshotItem))
            {
                if (gatheredItem.ItemId != oneshotItem)
                {
                    return false;
                }

                uint stageNo = StageManager.ConvertIdToStageNo(stageLayoutId);
                var stageSpots = Server.AssetRepository.GatheringSpotInfoAsset.GatheringInfoMap[stageNo];

                if (stageSpots.TryGetValue((stageLayoutId.GroupId, index), out var spotInfo)
                    && spotInfo.GatheringType == GatheringType.OM_GATHER_ONE_OFF)
                {
                    uint encodedUnlock = stageLayoutId.GroupId * 100 + index;
                    UnlockableItemCategory category = OneOffGatheringItemGenerator.OneOffGatherType[stageInfo];

                    client.Character.UnlockableItems.Add((category, encodedUnlock));
                    Server.Database.InsertUnlockedItem(client.Character.CharacterId, category, encodedUnlock, connectionIn);

                    if (client.Character.ActiveCharacterPlayPointData?.PlayPoint.ExpMode == ExpMode.PlayPoint)
                    {
                        queue.Enqueue(client, Server.PPManager.AddPlayPoint(client, (ONEOFF_GATHER_PP_AMOUNT, 0), type: 1, connectionIn: connectionIn));
                    }
                    else
                    {
                        queue.AddRange(Server.ExpManager.AddExp(client, client.Character, (ONEOFF_GATHER_EXP_AMOUNT, 0), RewardSource.None, connectionIn: connectionIn));
                    }

                    queue.AddRange(Server.AchievementManager.HandleSparkleCollect(client, stageInfo.AreaId, connectionIn));

                    return true;
                }

                return false;
            }
            return false;
        }
    }
}
