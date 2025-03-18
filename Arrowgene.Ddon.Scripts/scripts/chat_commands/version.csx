public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.User;
    public override string CommandName            => "version";
    public override string HelpText               => "usage: `/version` - Provides information about the running server version";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        responses.Add(ChatResponse.ServerChat(client, Util.GetVersion("GameServer")));
    }
}

return new ChatCommand();
