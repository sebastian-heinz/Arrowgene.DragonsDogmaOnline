#load "libs.csx"

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName            => "il";
    public override string HelpText               => "usage: `/il <itemname> [n?]` - Lookup item, no spaces.";

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
            if (int.TryParse(command[2], out int parsedCount))
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
            // Try an exact match first.
            if (Enum.TryParse(typeof(ItemId), command[0], true, out var parsedId))
            {
                responses.Add(ChatResponse.ServerChat(client, $"{command[0]}=>"));
                responses.Add(ChatResponse.ServerChat(client, $" {(uint)parsedId}:{(ItemId)parsedId}"));
                return;
            }

            // Fallback to fuzzy
            string input = command[0].ToLower();

            var matches = FuzzySearch.Search(input, Enum.GetNames(typeof(ItemId)))
                            .OrderBy(x => x.Score)
                            .Select(x => x.Item)
                            .ToList();
            if (matches.Count > 0)
            {
                matches = matches.Take(Math.Min(matches.Count, topCount)).ToList();
            }

            responses.Add(ChatResponse.ServerChat(client, $"{command[0]}=>"));
            foreach(var match in matches)
            {
                responses.Add(ChatResponse.ServerChat(client, $"{match}:{(uint) Enum.Parse(typeof(ItemId), match)}"));
            }
        }
    }
}

return new ChatCommand();
