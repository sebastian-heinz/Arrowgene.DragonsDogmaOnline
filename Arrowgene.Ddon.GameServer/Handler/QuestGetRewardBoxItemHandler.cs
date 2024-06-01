using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp.Consumer.BlockingQueueConsumption;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetRewardBoxItemHandler : GameStructurePacketHandler<C2SQuestGetRewardBoxItemReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetRewardBoxItemHandler));

        public QuestGetRewardBoxItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SQuestGetRewardBoxItemReq> packet)
        {
            // client.Send(GameFull.Dump_902);

            var rewardIndex = packet.Structure.ListNo;
            if (rewardIndex == 0 || rewardIndex > client.Character.QuestRewards.Count)
            {
                Logger.Error($"Illegal reward request sent to server.");
                client.Send(new S2CQuestGetRewardBoxItemRes() { Error = 1});
                return;
            }

            // Make zero based index
            var boxRewards = client.Character.QuestRewards[(int)(rewardIndex - 1)];

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = 0
            };

            foreach (var boxReward in packet.Structure.GetRewardBoxItemList)
            {
                var reward = boxRewards.Rewards[boxReward.UID];

                var result = Server.ItemManager.AddItem(Server, client.Character, StorageType.StorageBoxNormal, reward.ItemId, reward.Num);
                updateCharacterItemNtc.UpdateItemList.AddRange(result);
            }
            client.Send(updateCharacterItemNtc);

            // Remove this reward from the list
            client.Character.QuestRewards.RemoveAt((int)(rewardIndex - 1));

            client.Send(new S2CQuestGetRewardBoxItemRes());
        }
    }
}
