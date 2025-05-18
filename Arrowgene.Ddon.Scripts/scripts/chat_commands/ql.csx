using System.Collections.Generic;
using System;

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName => "ql";
    public override string HelpText => "usage: `/ql <QuestId>` - Lookup QuestScheduleId.";

    public static int LevenshteinDistance(string s, string t)
    {
        if (string.IsNullOrEmpty(s)) return string.IsNullOrEmpty(t) ? 0 : t.Length;
        if (string.IsNullOrEmpty(t)) return s.Length;

        int[,] distance = new int[s.Length + 1, t.Length + 1];

        for (int i = 0; i <= s.Length; i++)
            distance[i, 0] = i;
        for (int j = 0; j <= t.Length; j++)
            distance[0, j] = j;

        for (int i = 1; i <= s.Length; i++)
        {
            for (int j = 1; j <= t.Length; j++)
            {
                int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;

                distance[i, j] = Math.Min(
                    Math.Min(distance[i - 1, j] + 1,
                             distance[i, j - 1] + 1),
                    distance[i - 1, j - 1] + cost);
            }
        }

        return distance[s.Length, t.Length];
    }

    public static double Similarity(string s, string t)
    {
        int maxLen = Math.Max(s.Length, t.Length);
        if (maxLen == 0) return 1.0;

        int distance = LevenshteinDistance(s, t);
        return 1.0 - (double)distance / maxLen;
    }

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

                var matches = Enum.GetValues<QuestId>()
                    .SelectMany(x => QuestManager.GetQuestsByQuestId(x))
                    .Select(x => (x.QuestId, x.QuestScheduleId, Similarity: Similarity(Enum.GetName(x.QuestId).ToLower(), input)))
                    .OrderByDescending(x => x.Similarity)
                    .Take(topCount);

                responses.Add(ChatResponse.ServerChat(client, $"{command[0]}=>"));
                foreach (var match in matches)
                {
                    responses.Add(
                        ChatResponse.ServerChat(
                            client,
                            $"\t{match.QuestScheduleId} : ({match.QuestId}/{(uint)match.QuestId})"
                        )
                    );
                }
            }
        }
    }
}

return new ChatCommand();
