public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.User;
    public override string CommandName            => "help";
    public override string HelpText               => "usage: `/help` - Shows this text";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        var disableAccountCheckType = server.GameSettings.Get<bool>("ChatCommands", "DisableAccountTypeCheck");

        server.ScriptManager.ChatCommandModule.Commands.Values
            .Where(x => (client.Account.State >= x.AccountState) || disableAccountCheckType)
            .Select(cmd => new ChatResponse() {Message = cmd.HelpText})
            .ToList()
            .ForEach(response => responses.Add(response));
    }
}

return new ChatCommand();
