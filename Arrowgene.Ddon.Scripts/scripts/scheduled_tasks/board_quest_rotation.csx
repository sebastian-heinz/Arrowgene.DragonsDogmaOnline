public class BoardQuestRotationTask : DailyTask
{
    public BoardQuestRotationTask(uint hour, uint minute)
        : base(TaskType.BoardQuestRotation, hour, minute) { }

    public override void RunTask(DdonGameServer server)
    {
        foreach (var character in server.ClientLookup.GetAllCharacter())
        {
            foreach (var key in character.CompletedQuests.Keys
                .Where(QuestManager.IsBoardQuest)
                .ToList())
            {
                character.CompletedQuests.Remove(key);
            }
        }

        server.LightQuestManager.InsertRecordsFromAsset();

        var questRecords = server.Database.SelectLightQuestRecords();
        var extantQuests = QuestManager.GetQuestsByType(QuestType.Light);

        var quests = questRecords
            .Where(x => !extantQuests.Contains(x.QuestScheduleId))
            .Select(x => server.LightQuestManager.GenerateQuestFromRecord(x));

        QuestManager.AddQuests(server, quests);

        server.RpcManager.AnnounceOthers("internal/command", RpcInternalCommand.BoardQuestDailyRotation, null);
    }
}

return new BoardQuestRotationTask(5, 0);
