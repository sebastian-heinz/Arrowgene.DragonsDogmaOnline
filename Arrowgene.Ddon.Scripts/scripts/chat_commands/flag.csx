#load "libs.csx"

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName => "flag";
    public override string HelpText => "usage: `/flag <set|clear> <WorldManageLayout|WorldManageQuest> QuestId value` - Performs the operation using a quest flag";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        if (command.Length < 4)
        {
            responses.Add(ChatResponse.CommandError(client, "No arguments provided"));
            return;
        }

        try
        {
            var action = command[0].ToLower();
            var type = command[1].ToLower();
            var flagNo = Int32.Parse(command[3]);

            if (!Enum.TryParse(command[2], true, out QuestId questId))
            {
                responses.Add(ChatResponse.CommandError(client, "Failed to parse QuestId"));
                return;
            }

            var baseQuest = QuestManager.GetQuestByQuestId(QuestId.WorldManageDebug);
            var questResultCommands = new List<CDataQuestCommand>();
            switch (action)
            {
                case "set":
                    switch (type)
                    {
                        case "wml":
                            questResultCommands.Add(QuestManager.ResultCommand.WorldManageLayoutFlagOn(flagNo, (int)questId));
                            break;
                        case "wmq":
                            questResultCommands.Add(QuestManager.ResultCommand.WorldManageQuestFlagOn(flagNo, (int)questId));
                            break;
                        default:
                            responses.Add(ChatResponse.CommandError(client, $"Unexpected flag type '{command[1]}'."));
                            return;
                    }
                    break;
                case "clear":
                    switch (type)
                    {
                        case "wml":
                            questResultCommands.Add(QuestManager.ResultCommand.WorldManageLayoutFlagOff(flagNo, (int)questId));
                            break;
                        case "wmq":
                            questResultCommands.Add(QuestManager.ResultCommand.WorldManageQuestFlagOff(flagNo, (int)questId));
                            break;
                        default:
                            responses.Add(ChatResponse.CommandError(client, $"Unexpected flag type '{command[1]}'."));
                            return;
                    }
                    break;
                default:
                    responses.Add(ChatResponse.CommandError(client, $"Unknown action '{action}'."));
                    return;
            }

            // Force the quest to execute
            S2CQuestQuestProgressNtc progressNtc = new S2CQuestQuestProgressNtc()
            {
                ProgressCharacterId = client.Character.CharacterId,
                QuestScheduleId = baseQuest.QuestScheduleId,
                QuestProcessStateList = new List<CDataQuestProcessState>()
                {
                    new CDataQuestProcessState()
                    {
                        ProcessNo = 0,
                        SequenceNo = 0,
                        BlockNo = 2,
                        ResultCommandList = questResultCommands
                    }
                }
            };
            client.Party.SendToAll(progressNtc);

            // Reset quest so a command can be sent again
            S2CQuestQuestProgressNtc resetNtc = new S2CQuestQuestProgressNtc()
            {
                ProgressCharacterId = client.Character.CharacterId,
                QuestScheduleId = baseQuest.QuestScheduleId,
                QuestProcessStateList = new List<CDataQuestProcessState>()
                {
                    new CDataQuestProcessState()
                    {
                        ProcessNo = 0,
                        SequenceNo = 0,
                        BlockNo = 1
                    }
                }
            };
            client.Party.SendToAll(resetNtc);
        }
        catch (Exception)
        {
            responses.Add(ChatResponse.CommandError(client, "Invalid Arguments"));
        }
    }
}

return new ChatCommand();
