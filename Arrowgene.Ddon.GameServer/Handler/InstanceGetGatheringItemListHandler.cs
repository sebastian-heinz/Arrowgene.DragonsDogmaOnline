using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetGatheringItemListHandler : GameRequestPacketHandler<C2SInstanceGetGatheringItemListReq, S2CInstanceGetGatheringItemListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetGatheringItemListHandler));

        // TODO: Different chances for each gathering item type
        private static readonly double BREAK_CHANCE = 0.1;

        public InstanceGetGatheringItemListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CInstanceGetGatheringItemListRes Handle(GameClient client, C2SInstanceGetGatheringItemListReq request)
        {
            bool isGatheringItemBreak = false;
            var (isNew, gatheringItems) = client.InstanceGatheringItemManager.FetchOrGenerate(request.LayoutId, request.PosId);

            PacketQueue queue = new();

            Server.Database.ExecuteInTransaction(connection =>
            {
                if (isNew && request.GatheringItemUId.Any())
                {
                    var gatheringItem = Server.ItemManager.LookupInfoByUID(Server, request.GatheringItemUId, connection);

                    switch ((ItemId)gatheringItem.ItemId)
                    {
                        case ItemId.Pickaxe:
                        case ItemId.ArtisansPickaxe:
                        case ItemId.EnhancedPickaxe:
                            queue.AddRange(Server.AchievementManager.HandleCollect(client, AchievementCollectParam.Ore, connection));
                            break;
                        case ItemId.Lockpick:
                        case ItemId.AllPurposeLockpick:
                        case ItemId.EnhancedLockpick:
                            queue.AddRange(Server.AchievementManager.HandleCollect(client, AchievementCollectParam.Chest, connection));
                            break;
                        case ItemId.LumberKnife:
                        case ItemId.ArtisansLumberKnife:
                        case ItemId.EnhancedLumberKnife:
                            queue.AddRange(Server.AchievementManager.HandleCollect(client, AchievementCollectParam.Wood, connection));
                            break;
                    }

                    if (Random.Shared.NextDouble() < BREAK_CHANCE)
                    {
                        isGatheringItemBreak = true;

                        S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc();
                        ntc.UpdateItemList.AddRange(Server.ItemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, ItemManager.ItemBagStorageTypes, request.GatheringItemUId, 1, connection));
                        client.Enqueue(ntc, queue);
                    }
                }

            });

            queue.Send();
            S2CInstanceGetGatheringItemListRes res = new()
            {
                LayoutId = request.LayoutId,
                PosId = request.PosId,
                GatheringItemUId = request.GatheringItemUId,
                IsGatheringItemBreak = isGatheringItemBreak,
                ItemList = gatheringItems
                .Select((asset, index) => new CDataGatheringItemElement()
                {
                    SlotNo = (uint)index,
                    ItemId = (uint) asset.ItemId,
                    ItemNum = asset.ItemNum,
                    Quality = asset.Quality,
                    IsHidden = asset.IsHidden
                })
                .ToList()
            };

            return res;
        }
    }
}
