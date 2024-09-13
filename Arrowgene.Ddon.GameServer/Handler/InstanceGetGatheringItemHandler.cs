#nullable enable
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetGatheringItemHandler : GameStructurePacketHandler<C2SInstanceGetGatheringItemReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetGatheringItemHandler));

        public InstanceGetGatheringItemHandler(DdonGameServer server) : base(server)
        {
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
                        gatheredItem = client.InstanceBbmItemManager.FetchBitterblackItems(Server, client, stageId, posId)[(int)gatheringItemRequest.SlotNo];
                    }
                    else
                    {
                        gatheredItem = client.InstanceGatheringItemManager.GetAssets(req.Structure.LayoutId, (int)req.Structure.PosId)[(int)gatheringItemRequest.SlotNo];
                    }

                    Server.ItemManager.GatherItem(Server, client.Character, ntc, gatheredItem, gatheringItemRequest.Num, connection);
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
                var itemInfo = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, ntc.UpdateItemList[0].ItemList.ItemId);
                var equipInfo = new CDataCharacterEquipInfo()
                {
                    EquipItemUId = ntc.UpdateItemList[0].ItemList.ItemUId,
                    EquipCategory = (byte)itemInfo.EquipSlot,
                    EquipType = EquipType.Performance,
                };

                (S2CItemUpdateCharacterItemNtc itemNtc, S2CEquipChangeCharacterEquipNtc equipNtc) equipResult = (null, null);
                Server.Database.ExecuteInTransaction(connection =>
                {
                    equipResult =((S2CItemUpdateCharacterItemNtc itemNtc, S2CEquipChangeCharacterEquipNtc equipNtc)) Server.EquipManager.HandleChangeEquipList(
                        Server,
                        client,
                        client.Character,
                        new List<CDataCharacterEquipInfo>() { equipInfo },
                        ItemNoticeType.GatherEquipItem,
                        new List<StorageType>() { StorageType.ItemBagEquipment },
                        connection);
                });
                client.Send(equipResult.itemNtc);

                // TODO: Can this be optimized? So only players who need to see
                //       get the update?
                foreach (var otherClient in Server.ClientLookup.GetAll())
                {
                    otherClient.Send(equipResult.equipNtc);
                }
            }
        }
    }
}
