#load "libs/ScriptUtils.csx"

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName => "group";
    public override string HelpText => "usage: `/group <reset|destroy|annihilate|appear> StageId.LayerNo.GroupId [SubgroupId]` - Performs operations on an enemy group";

    private List<Quest> FindQuestScheduleIdForStageLayoutId(GameClient client, StageLayoutId stageLayoutId, byte subGroupId)
    {
        var results = new List<Quest>();
        foreach (var questScheduleId in QuestManager.CollectQuestScheduleIds(client, stageLayoutId))
        {
            var quest = QuestManager.GetQuestByScheduleId(questScheduleId);

            var questStateManager = QuestManager.GetQuestStateManager(client, quest);
            if (quest.OverrideEnemySpawn && quest.HasEnemiesInCurrentStageGroup(stageLayoutId))
            {
                results.Add(quest);
            }
            else if (!quest.OverrideEnemySpawn && questStateManager.HasEnemiesForCurrentQuestStepInStageGroup(quest, stageLayoutId, subGroupId))
            {
                results.Add(quest);
            }
        }
        return results;
    }

    private void ResetGroup(GameClient client, CDataStageLayoutId layoutId, byte subGroupId)
    {
        foreach (var enemy in client.Party.InstanceEnemyManager.GetInstancedEnemies(layoutId.AsStageLayoutId()))
        {
            var uid = ContextManager.CreateEnemyUID(enemy.Index, layoutId);
            ContextManager.RemoveContext(client.Party, uid);
        }
        client.Party.InstanceEnemyManager.ResetEnemyNode(layoutId.AsStageLayoutId());

        var quests = FindQuestScheduleIdForStageLayoutId(client, layoutId.AsStageLayoutId(), subGroupId);
        foreach (var quest in quests)
        {
            quest.PopulateStartingEnemyData(QuestManager.GetQuestStateManager(client, quest));
        }

        client.Send(new S2CInstanceEnemyGroupResetNtc() { LayoutId = layoutId });
    }

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        if (command.Length < 2)
        {
            responses.Add(ChatResponse.CommandError(client, "No arguments provided"));
            return;
        }

        try
        {
            var action = command[0].ToLower();
            var layoutId = ScriptUtils.ParseLayoutId(command[1]);

            byte subGroupId = 0;
            if (command.Length >= 3)
            {
                subGroupId = byte.Parse(command[2]);
            }

            Console.WriteLine($"{layoutId}.{subGroupId}");

            switch (action)
            {
                case "reset":
                    ResetGroup(client, layoutId, subGroupId);
                    break;
                case "destroy":
                    client.Send(new S2CInstanceEnemyGroupDestroyNtc() { LayoutId = layoutId });
                    ResetGroup(client, layoutId, subGroupId);
                    break;
                case "annihilate":
                    client.Send(new S2CInstanceEnemyStageBossAnnihilateNtc() { LayoutId = layoutId });
                    break;
                case "appear":
                    client.Send(new S2CInstanceEnemySubGroupAppearNtc() { LayoutId = layoutId, SubGroupId = subGroupId });
                    break;
                default:
                    responses.Add(ChatResponse.CommandError(client, $"Unknown action '{action}'."));
                    break;
            }
        }
        catch (Exception)
        {
            responses.Add(ChatResponse.CommandError(client, "Invalid Arguments"));
        }
    }
}

return new ChatCommand();
