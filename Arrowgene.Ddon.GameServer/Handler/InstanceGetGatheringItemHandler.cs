#nullable enable
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;
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
            var stageId = request.LayoutId.AsStageId();
            var (_, itemList) = client.InstanceGatheringItemManager.FetchOrGenerate(stageId, posId);

            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (CDataGatheringItemGetRequest gatheringItemRequest in request.GatheringItemGetRequestList)
                {
                    InstancedGatheringItem gatheredItem = itemList.ElementAtOrDefault((int)gatheringItemRequest.SlotNo)
                        ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_INSTANCE_AREA_INVALID_GATHERING_ITEM_POS_ID,
                        $"Invalid gathering item index at {stageId}.{posId}");
                    
                    Server.ItemManager.GatherItem(client.Character, ntc, gatheredItem, gatheringItemRequest.Num, connection);
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
                var itemInfo = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, ntc.UpdateItemList[0].ItemList.ItemId);
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
    }
}
