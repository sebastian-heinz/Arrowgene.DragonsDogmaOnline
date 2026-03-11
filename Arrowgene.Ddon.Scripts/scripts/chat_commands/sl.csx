using System;
using System.Reflection;

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName            => "sl";
    public override string HelpText               => "usage: `/sl <stagename> [n?]` - Lookup stage, no spaces.";

    private readonly Dictionary<string, StageInfo> StageDictionary;

    public ChatCommand()
    {
        StageDictionary = new();
        var fields = typeof(Stage).GetFields(BindingFlags.Static | BindingFlags.Public).Where(fi => fi.FieldType == typeof(StageInfo));
        foreach(var field in fields)
        {
            StageDictionary[field.Name.ToLower()] = (StageInfo)field.GetValue(null);
        }
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
            string input = command[0].ToLower();

            // Try an exact match first.
            if (StageDictionary.TryGetValue(input, out var match))
            {
                responses.Add(ChatResponse.ServerChat(client, $"{command[0]}=>"));
                responses.Add(ChatResponse.ServerChat(client, $" i{match.StageId}/n{match.StageNo}:{match.Name}"));
                return;
            }

            // Fallback to fuzzy
            var matches = FuzzySearch.Search(input, StageDictionary.Keys)
                            .OrderBy(x => x.Score)
                            .Select(x => StageDictionary[x.Item])
                            .ToList();
            if (matches.Count > 0)
            {
                matches = matches.Take(Math.Min(matches.Count, topCount)).ToList();
            }

            responses.Add(ChatResponse.ServerChat(client, $"{command[0]}=>"));
            foreach (var fmatch in matches)
            {
                responses.Add(ChatResponse.ServerChat(client, $" i{fmatch.StageId}/n{fmatch.StageNo}:{fmatch.Name}"));
            }
        }
    }
}

return new ChatCommand();
