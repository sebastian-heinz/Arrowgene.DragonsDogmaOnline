using System;
using System.Collections.Generic;

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName => "finishquesttype";
    public override string HelpText => "usage: `/finishquesttype [type]` - Finish all quests.";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        if (command.Length == 0)
        {
            responses.Add(ChatResponse.CommandError(client, "No arguments provided."));
            return;
        }

        if (Enum.TryParse(command[0], out QuestType parsedQuestType))
        {
            server.Database.ExecuteInTransaction(connection =>
            {
                foreach (var bar in QuestManager.GetQuestsByType(parsedQuestType))
                {
                    var quest = QuestManager.GetQuestByScheduleId(bar);
                    if (!client.Character.HasQuestCompleted(quest.QuestId))
                    {
                        server.Database.InsertCompletedQuest(client.Character.CommonId, quest.QuestId, quest.QuestType, connection);
                    }
                }
            });
        }
        else
        {
            responses.Add(ChatResponse.CommandError(client, $"Invalid quest type \"{command[0]}\"."));
            return;
        }

        responses.Add(ChatResponse.ServerMessage(client, $"All quests of type {command[0]} completed. Relog."));
    }
}

return new ChatCommand();
