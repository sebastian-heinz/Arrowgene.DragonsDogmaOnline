using Arrowgene.Ddon.GameServer.Quests;

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName            => "finishquest";
    public override string HelpText               => "usage: `/finishquest [questScheduleId]` - Finish quests.";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        if (command.Length == 0)
        {
            responses.Add(ChatResponse.CommandError(client, "No arguments provided."));
            return;
        }

        uint questScheduleId = 0;
        // Try by id
        if (uint.TryParse(command[0], out uint parsedQuestScheduleId))
        {
            questScheduleId = parsedQuestScheduleId;
        }
        else
        {
            responses.Add(ChatResponse.CommandError(client, $"Invalid questId \"{command[0]}\". It must be a number."));
            return;
        }

        Quest quest = QuestManager.GetQuestByScheduleId(parsedQuestScheduleId);
        if (quest is null)
        {
            responses.Add(ChatResponse.CommandError(client, $"Invalid questId \"{command[0]}\". This quest does not exist."));
            return;
        }
        //Super jank. Leaves lots of red icons over peoples heads, but doesn't immediately require relogs.

        S2CQuestCompleteNtc completeNtc = new S2CQuestCompleteNtc()
        {
            QuestScheduleId = quest.QuestScheduleId,
            RandomRewardNum = quest.RandomRewardNum(),
            ChargeRewardNum = quest.RewardParams.ChargeRewardNum,
            ProgressBonusNum = quest.RewardParams.ProgressBonusNum,
            IsRepeatReward = quest.RewardParams.IsRepeatReward,
            IsUndiscoveredReward = quest.RewardParams.IsUndiscoveredReward,
            IsHelpReward = quest.RewardParams.IsHelpReward,
            IsPartyBonus = quest.RewardParams.IsPartyBonus,
        };

        if (quest.IsPersonal)
        {
            client.QuestState.CompleteQuest(quest.QuestScheduleId);
            client.Send(completeNtc);
        }
        else
        {
            client.Party.QuestState.CompleteQuestProgress(quest.QuestScheduleId);
            client.Party.QuestState.UpdatePriorityQuestList(client).Send();
            client.Party.SendToAll(completeNtc);

            if (quest.ResetPlayerAfterQuest)
            {
                foreach (var memberClient in client.Party.Clients)
                {
                    server.CharacterManager.UpdateCharacterExtendedParamsNtc(memberClient, memberClient.Character);
                }
            }
        }
        

        responses.Add(ChatResponse.ServerMessage(client, $"Finishing {quest.QuestId.ToString()} ({quest.QuestId})."));
    }
}

return new ChatCommand();