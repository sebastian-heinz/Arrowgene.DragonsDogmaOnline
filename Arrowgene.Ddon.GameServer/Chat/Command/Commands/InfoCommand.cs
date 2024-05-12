using System.Collections.Generic;
using System.Text;
using Arrowgene.Ddon.Database.Model;

namespace Arrowgene.Ddon.GameServer.Chat.Command.Commands
{
    /// <summary>
    /// Information about yourself
    /// </summary>
    public class InfoCommand : ChatCommand
    {
        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "info";
        public override string HelpText => "usage: `/info` - Print details about where you are, and other values";

        public override void Execute(string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append($"StageId:{client.Character.Stage.Id} ");
            sb.Append($"LayerNo:{client.Character.Stage.LayerNo} ");
            sb.Append($"GroupId:{client.Character.Stage.GroupId} ");
            sb.Append($"StageNo:{client.Character.StageNo} ");
            sb.Append($"Pos:[X:{client.Character.X} Y:{client.Character.Y} Z:{client.Character.Z}]");


            ChatResponse response = new ChatResponse();
            response.Message = sb.ToString();
            responses.Add(response);
        }
    }
}
