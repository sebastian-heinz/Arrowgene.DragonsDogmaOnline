#nullable enable
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetGatheringItemHandler : StructurePacketHandler<GameClient, C2SInstanceGetGatheringItemReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetGatheringItemHandler));

        private readonly ItemManager _itemManager;
        private readonly DdonGameServer _Server;

        public InstanceGetGatheringItemHandler(DdonGameServer server) : base(server)
        {
            this._itemManager = server.ItemManager;
            this._Server = server;
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceGetGatheringItemReq> req)
        {
            S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = req.Structure.EquipToCharacter == 0 ? ItemNoticeType.Gather : ItemNoticeType.GatherEquipItem
            };

            Server.Database.ExecuteInTransaction(connection =>
            {
                uint posId = req.Structure.PosId;
                var stageId = req.Structure.LayoutId.AsStageId();
                foreach (CDataGatheringItemGetRequest gatheringItemRequest in req.Structure.GatheringItemGetRequestList)
                {
                    InstancedGatheringItem gatheredItem;

                    if (StageManager.IsBitterBlackMazeStageId(stageId))
                    {
                        gatheredItem = client.InstanceBbmItemManager.FetchBitterblackItems(stageId, posId)[(int)gatheringItemRequest.SlotNo];
                    }
                    else
                    {
                        gatheredItem = client.InstanceGatheringItemManager.GetAssets(req.Structure.LayoutId, req.Structure.PosId)[(int)gatheringItemRequest.SlotNo];
                    }

                    this._itemManager.GatherItem(Server, client.Character, ntc, gatheredItem, gatheringItemRequest.Num, connection);
                }
            });

            client.Send(ntc);

            S2CInstanceGetGatheringItemRes res = new S2CInstanceGetGatheringItemRes();
            res.LayoutId = req.Structure.LayoutId;
            res.PosId = req.Structure.PosId;
            res.GatheringItemGetRequestList = req.Structure.GatheringItemGetRequestList;
            client.Send(res);

            if (req.Structure.EquipToCharacter == 1)
            {
                var itemInfo = ClientItemInfo.GetInfoForItemId(_Server.AssetRepository.ClientItemInfos, ntc.UpdateItemList[0].ItemList.ItemId);
                var equipInfo = new CDataCharacterEquipInfo()
                {
                    EquipItemUId = ntc.UpdateItemList[0].ItemList.ItemUId,
                    EquipCategory = (byte)itemInfo.EquipSlot,
                    EquipType = EquipType.Performance,
                };

                (S2CItemUpdateCharacterItemNtc itemNtc, S2CEquipChangeCharacterEquipNtc equipNtc) equipResult = (null, null);
                Server.Database.ExecuteInTransaction(connection =>
                {
                    equipResult = ((S2CItemUpdateCharacterItemNtc itemNtc, S2CEquipChangeCharacterEquipNtc equipNtc)) _Server.EquipManager.HandleChangeEquipList(
                        _Server,
                        client,
                        client.Character,
                        new List<CDataCharacterEquipInfo>() { equipInfo },
                        ItemNoticeType.GatherEquipItem,
                        new List<StorageType>() { StorageType.ItemBagEquipment },
                        connection);
                });
                client.Send(equipResult.itemNtc);
            }
        }
    }
}
