using System.Collections.Generic;
using System;

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName => "ql";
    public override string HelpText => "usage: `/ql <QuestId>` - Lookup QuestScheduleId.";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        if (command.Length == 0)
        {
            responses.Add(ChatResponse.CommandError(client, "No arguments provided."));
            return;
        }

        int topCount = 5;
        if (command.Length >= 2)
        {
            if (int.TryParse(command[1], out int parsedCount))
            {
                topCount = parsedCount;
            }
            else
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid count \"{command[1]}\". It must be a number."));
                return;
            }
        }

        if (command.Length >= 1)
        {
            QuestId questId = 0;
            // Try an exact match; first by number then by string lookup.
            if (UInt32.TryParse(command[0], out uint parsedRawId))
            {
                questId = (QuestId)parsedRawId;

                var matches = QuestManager.GetQuestsByQuestId(questId).Where(x => x.Enabled).ToList().OrderBy(x => (uint)x.QuestScheduleId);
                responses.Add(ChatResponse.ServerChat(client, $"{command[0]}=>"));
                foreach (var match in matches)
                {
                    responses.Add(ChatResponse.ServerChat(client, $"\t{match.QuestScheduleId}"));
                }
            }
            else if (Enum.TryParse(typeof(QuestId), command[0], true, out var parsedId))
            {
                questId = (QuestId)parsedId;

                var matches = QuestManager.GetQuestsByQuestId(questId).Where(x => x.Enabled).ToList().OrderBy(x => (uint)x.QuestScheduleId);
                responses.Add(ChatResponse.ServerChat(client, $"{command[0]}=>"));
                foreach (var match in matches)
                {
                    responses.Add(ChatResponse.ServerChat(client, $"\t{match.QuestScheduleId}"));
                }
            }
            else
            {
                // Fallback to fuzzy
                string input = command[0].ToLower();

                var matches = FuzzySearch.Search(input, Enum.GetNames<QuestId>())
                    .OrderBy(x => x.Score)
                    .Select(x => x.Item)
                    .ToList();

                if (matches.Count > 0)
                {
                    matches = matches.Take(Math.Min(matches.Count, topCount)).ToList();
                }

                responses.Add(ChatResponse.ServerChat(client, $"{command[0]}=>"));
                foreach (var match in matches)
                {
                    QuestId matchEnum = Enum.Parse<QuestId>(match);
                    foreach (var scheduleId in QuestManager.GetQuestsByQuestId(matchEnum))
                    {
                        responses.Add(
                            ChatResponse.ServerChat(
                                client,
                                $"\t{scheduleId} : ({matchEnum}/{(uint)matchEnum})"
                            )
                        );
                    }
                }
            }
        }
    }
}

return new ChatCommand();
