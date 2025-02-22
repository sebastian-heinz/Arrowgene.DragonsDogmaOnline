using System.Text;

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.User;
    public override string CommandName            => "info";
    public override string HelpText               => "usage: `/info` - Print details about where you are, and other values";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"StageId: {client.Character.Stage} ");
        sb.Append($"StageNo: {client.Character.StageNo} ");
        sb.Append($"Pos:[X:{client.Character.X:F4} Y:{client.Character.Y:F4} Z:{client.Character.Z:F4}]");

        ChatResponse response = new ChatResponse();
        response.Message = sb.ToString();
        responses.Add(response);
    }
}

return new ChatCommand();
