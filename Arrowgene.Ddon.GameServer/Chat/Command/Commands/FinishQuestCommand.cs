using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model.Quest;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Chat.Command.Commands
{
    public class FinishQuestCommand : ChatCommand
    {
        public override AccountStateType AccountState => AccountStateType.User;

        public override string Key => "finishquest";
        public override string HelpText => "usage: `/finishquest [questid]` - Finish quests.";

        private DdonGameServer _server;

        public FinishQuestCommand(DdonGameServer server)
        {
            _server = server;
        }

        public override void Execute(string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
        {
            if (command.Length == 0)
            {
                responses.Add(ChatResponse.CommandError(client, "No arguments provided."));
                return;
            }

            QuestId? questId = null;
            // Try by id
            if (uint.TryParse(command[0], out uint parsedQuestId))
            {
                questId = (QuestId)parsedQuestId;
            }
            else
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid questId \"{command[0]}\". It must be a number."));
                return;
            }

            Quest quest = QuestManager.GetQuest((QuestId)questId);

            if (quest is null)
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid questId \"{command[0]}\". This quest does not exist."));
                return;
            }

            //Super jank. Leaves lots of red icons over peoples heads, but doesn't immediately require relogs.
            client.Party.QuestState.CompletePartyQuestProgress(_server, client.Party, quest.QuestId);
            S2CQuestCompleteNtc completeNtc = new S2CQuestCompleteNtc()
            {
                QuestScheduleId = (uint)quest.QuestId,
                RandomRewardNum = quest.RandomRewardNum(),
                ChargeRewardNum = quest.RewardParams.ChargeRewardNum,
                ProgressBonusNum = quest.RewardParams.ProgressBonusNum,
                IsRepeatReward = quest.RewardParams.IsRepeatReward,
                IsUndiscoveredReward = quest.RewardParams.IsUndiscoveredReward,
                IsHelpReward = quest.RewardParams.IsHelpReward,
                IsPartyBonus = quest.RewardParams.IsPartyBonus,
            };
            client.Party.SendToAll(completeNtc);

            client.Party.QuestState.UpdatePriorityQuestList(_server, client.Party);

            if (quest.ResetPlayerAfterQuest)
            {
                foreach (var memberClient in client.Party.Clients)
                {
                    _server.CharacterManager.UpdateCharacterExtendedParamsNtc(memberClient, memberClient.Character);
                }
            }

            responses.Add(ChatResponse.ServerMessage(client, $"Finishing {questId.ToString()} ({questId})."));
        }
    }
}
