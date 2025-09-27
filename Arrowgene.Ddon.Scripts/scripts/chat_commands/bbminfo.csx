using System.Collections.Generic;
using System.Text;

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.User;
    public override string CommandName => "bbminfo";
    public override string HelpText => "usage: `/bbminfo` - Print details about your BBM progress.";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        if (client.Character.GameMode != GameMode.BitterblackMaze)
        {
            responses.Add(ChatResponse.CommandError(client, $"You can only use this command while in Bitterblack Maze or the Cove."));
            //return;
        }
        client.Send(new S2CConnectionInformationNtc()
        {
            ParagraphList = [.. server.BitterblackMazeManager.GenerateProgressReportString(client).Select((x, i) => new CDataInformationParagraph()
                {
                    Index = (uint)i,
                    Text = x,
                })]
        });
    }
}

return new ChatCommand();
