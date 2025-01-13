using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetGatheringItemListHandler : GameStructurePacketHandler<C2SInstanceGetGatheringItemListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetGatheringItemListHandler));

        // TODO: Different chances for each gathering item type
        private static readonly double BREAK_CHANCE = 0.1;

        public InstanceGetGatheringItemListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceGetGatheringItemListReq> req)
        {
            bool isGatheringItemBreak = false;
            if(!client.InstanceGatheringItemManager.HasAssetsInstanced(req.Structure.LayoutId, (int)req.Structure.PosId) && req.Structure.GatheringItemUId.Length > 0 && Random.Shared.NextDouble() < BREAK_CHANCE)
            {
                isGatheringItemBreak = true;

                S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc();
                ntc.UpdateItemList.AddRange(Server.ItemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, ItemManager.ItemBagStorageTypes, req.Structure.GatheringItemUId, 1));
                client.Send(ntc);
            }

            List<InstancedGatheringItem> gatheringItems = new List<InstancedGatheringItem>();

            uint posId = req.Structure.PosId;
            var stageId = req.Structure.LayoutId.AsStageId();
            if (StageManager.IsBitterBlackMazeStageId(stageId))
            {
                gatheringItems.AddRange(client.InstanceBbmGatheringItemManager.FetchBitterblackItems(Server, client, stageId, posId));
            }
            else if (StageManager.IsEpitaphRoadStageId(stageId))
            {
                gatheringItems.AddRange(client.InstanceEpiGatheringManager.FetchItems(client, stageId, posId));
            }
            else
            {
                gatheringItems.AddRange(client.InstanceGatheringItemManager.GetAssets(req.Structure.LayoutId, (int)posId));
            }

            S2CInstanceGetGatheringItemListRes res = new S2CInstanceGetGatheringItemListRes();
            res.LayoutId = req.Structure.LayoutId;
            res.PosId = req.Structure.PosId;
            res.GatheringItemUId = req.Structure.GatheringItemUId;
            res.IsGatheringItemBreak = isGatheringItemBreak;
            res.Unk0 = false;
            res.Unk1 = new List<CDataGatheringItemListUnk1>();
            res.ItemList = gatheringItems
                .Select((asset, index) => new CDataGatheringItemElement()
                {
                    SlotNo = (uint) index,
                    ItemId = asset.ItemId,
                    ItemNum = asset.ItemNum,
                    Quality = asset.Quality,
                    IsHidden = asset.IsHidden
                })
                .ToList();
            client.Send(res);
        }
    }
}
