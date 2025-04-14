public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName            => "il";
    public override string HelpText               => "usage: `/il <itemname> [n?]` - Lookup item, no spaces.";

    static int LevenshteinDistance(string s, string t)
    {
        int n = s.Length;
        int m = t.Length;
        int[,] d = new int[n + 1, m + 1];

        // Verify arguments.
        if (n == 0)
        {
            return m;
        }

        if (m == 0)
        {
            return n;
        }

        // Initialize arrays.
        for (int i = 0; i <= n; d[i, 0] = i++)
        {
        }

        for (int j = 0; j <= m; d[0, j] = j++)
        {
        }

        // Begin looping.
        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                // Compute cost.
                int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                d[i, j] = Math.Min(
                Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                d[i - 1, j - 1] + cost);
            }
        }
        // Return cost.
        return d[n, m];
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
            // Try an exact match first.
            if (Enum.TryParse(typeof(ItemId), command[0], true, out var parsedId))
            {
                responses.Add(ChatResponse.ServerChat(client, $"{command[0]}=>"));
                responses.Add(ChatResponse.ServerChat(client, $" {(uint)parsedId}:{(ItemId)parsedId}"));
                return;
            }

            // Fallback to fuzzy
            string input = command[0].ToLower();

            var matches = Enum.GetValues(typeof(ItemId))
                .Cast<int>()
                .Select(x => (x, LevenshteinDistance(Enum.GetName(typeof(ItemId), x), input)))
                .OrderBy(x => x.Item2)
                .Take(topCount);

            responses.Add(ChatResponse.ServerChat(client, $"{command[0]}=>"));
            foreach(var match in matches)
            {
                responses.Add(ChatResponse.ServerChat(client, $"{match.Item1}:{Enum.GetName(typeof(ItemId), match.Item1)}"));
            }
        }
    }
}

return new ChatCommand();
