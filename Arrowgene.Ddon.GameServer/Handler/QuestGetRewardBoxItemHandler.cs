using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp.Consumer.BlockingQueueConsumption;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetRewardBoxItemHandler : GameRequestPacketHandler<C2SQuestGetRewardBoxItemReq, S2CQuestGetRewardBoxItemRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetRewardBoxItemHandler));

        public QuestGetRewardBoxItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetRewardBoxItemRes Handle(GameClient client, C2SQuestGetRewardBoxItemReq packet)
        {
            // client.Send(GameFull.Dump_902);

            var questBoxRewards = Server.RewardManager.GetQuestBoxRewards(client);

            var rewardIndex = packet.ListNo;
            if (rewardIndex == 0 || rewardIndex > questBoxRewards.Count)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_QUEST_NOT_EXIST_REWARD_BOX_LIST_NO, $"Illegal reward request sent to server.");
            }

            // Make zero based index
            var questBoxReward = questBoxRewards[(int)(rewardIndex - 1)];
            var rewards = Quest.AsCDataRewardBoxItems(questBoxReward);

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = 0
            };

            // If a quest has multiple slots with the same reward, coalesce them into a single slot.
            // This can happen when a quest has multiple random rewards and they random to the same item and amount
            Dictionary<string, CDataRewardBoxItem> coalescedRewards = new Dictionary<string, CDataRewardBoxItem>();
            foreach (var reward in rewards)
            {
                if (!coalescedRewards.ContainsKey(reward.UID))
                {
                    coalescedRewards[reward.UID] = reward;
                }
                else
                {
                    coalescedRewards[reward.UID].Num += reward.Num;
                }
            }

            var distinctRewards = packet.GetRewardBoxItemList.Select(x => x.UID).Distinct().ToList();

            var slotCount = coalescedRewards.Sum(x => distinctRewards.Contains(x.Key) ? Server.ItemManager.PredictAddItemSlots(client.Character, StorageType.StorageBoxNormal, (uint) x.Value.ItemId, x.Value.Num) : 0);
            if (slotCount > client.Character.Storage.GetStorage(StorageType.StorageBoxNormal).EmptySlots())
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_STORAGE_OVERFLOW);
            }

            PacketQueue queue = new();
            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (var rewardUID in distinctRewards)
                {
                    var reward = coalescedRewards[rewardUID];

                    var (specialQueue, isSpecial) = Server.ItemManager.HandleSpecialItem(client, updateCharacterItemNtc, reward.ItemId, reward.Num, connection);
                    if (isSpecial)
                    {
                        queue.AddRange(specialQueue);
                    }
                    else if (reward.Num > 0)
                    {
                        var result = Server.ItemManager.AddItem(Server, client.Character, false, (uint) reward.ItemId, reward.Num, connectionIn: connection);
                        updateCharacterItemNtc.UpdateItemList.AddRange(result);
                    }
                }
                Server.RewardManager.DeleteQuestBoxReward(client, questBoxReward.UniqRewardId, connectionIn: connection);
            });

            client.Send(updateCharacterItemNtc);
            queue.Send();

            return new S2CQuestGetRewardBoxItemRes();
        }
    }
}
