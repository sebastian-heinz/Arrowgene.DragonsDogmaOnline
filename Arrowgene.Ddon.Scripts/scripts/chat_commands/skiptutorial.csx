using Arrowgene.Ddon.GameServer.Chat.Command;

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName => "skiptutorial";
    public override string HelpText => "usage: `/skiptutorial` - Progress account to just after unlocking a pawn with all warps released";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        var handler = LibDdon.GetHandler<ChatCommandHandler>();
        handler.Handle(client, new() { Message = "/finishquest 1" }, responses);
        handler.Handle(client, new() { Message = "/finishquest 2" }, responses);
        handler.Handle(client, new() { Message = "/finishquest 3" }, responses);
        handler.Handle(client, new() { Message = "/finishquest 4" }, responses);
        handler.Handle(client, new() { Message = "/finishquest 26" }, responses);
        handler.Handle(client, new() { Message = "/givepawn" }, responses);
        handler.Handle(client, new() { Message = "/release" }, responses);
    }
}

return new ChatCommand();
