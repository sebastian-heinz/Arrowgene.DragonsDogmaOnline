using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemGetValuableItemListHandler : GameRequestPacketHandler<C2SItemGetValuableItemListReq, S2CItemGetValuableItemListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemGetValuableItemListHandler));


        public ItemGetValuableItemListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CItemGetValuableItemListRes Handle(GameClient client, C2SItemGetValuableItemListReq request)
        {
            var res = new S2CItemGetValuableItemListRes();

            foreach (var storageType in ItemManager.BothStorageTypes)
            {
                var storage = client.Character.Storage.GetStorage(storageType);
                res.EmptySlotNumList.Add(new CDataStorageEmptySlotNum()
                {
                    StorageType = storageType,
                    Slots = storage.EmptySlots()
                });
            }

            var rewards = Server.RewardManager.GetQuestBoxRewards(client).Select(x => QuestManager.GetQuestByScheduleId(x.QuestScheduleId).QuestId).ToHashSet();

            foreach (var valuableItem in ValuableItemQuestRewards)
            {
                if (client.Character.HasQuestCompleted(valuableItem.QuestId)
                    && !rewards.Contains(valuableItem.QuestId)
                    && client.Character.Storage.FindItemsByIdInStorage(ItemManager.EquipmentStorages, valuableItem.ItemId).Count == 0)
                {
                    res.ValuableItems.Add(new()
                    {
                        ItemId = valuableItem.ItemId,
                        WalletType = WalletType.Gold,
                        Price = (uint)(Server.AssetRepository.ClientItemInfos[valuableItem.ItemId].Price * 100)
                    });
                }
            }

            return res;
        }

        private static readonly List<(QuestId QuestId, ItemId ItemId)> ValuableItemQuestRewards =
        [
            (QuestId.VocationEmblemTrialFighter, ItemId.EmblemStoneFighter),
            (QuestId.VocationEmblemTrialPriest, ItemId.EmblemStonePriest),
            (QuestId.VocationEmblemTrialHunter, ItemId.EmblemStoneHunter),
            (QuestId.VocationEmblemTrialShieldSage, ItemId.EmblemStoneShieldSage),
            (QuestId.VocationEmblemTrialSeeker, ItemId.EmblemStoneSeeker),
            (QuestId.VocationEmblemTrialSorcerer, ItemId.EmblemStoneSorcerer),
            (QuestId.VocationEmblemTrialElementArcher, ItemId.EmblemStoneElementArcher),
            (QuestId.VocationEmblemTrialWarrior, ItemId.EmblemStoneWarrior),
            (QuestId.VocationEmblemTrialAlchemist, ItemId.EmblemStoneAlchemist),
            (QuestId.VocationEmblemTrialSpiritLancer, ItemId.EmblemStoneSpiritLancer),
            (QuestId.VocationEmblemTrialHighScepter, ItemId.EmblemStoneHighScepter),
        ];
    }
}
