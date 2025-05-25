using System.Collections.Generic;
using System.Text;

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.User;
    public override string CommandName => "party";
    public override string HelpText => "usage: `/party` - Print details about the party.";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        foreach (var member in client.Party.Members)
        {
            var memberString = "";
            if (member is PlayerPartyMember playerMember)
            {
                var character = playerMember.Client.Character;
                memberString = $"{member.MemberIndex} : {character.FirstName} {character.LastName}";
            }
            else if (member is PawnPartyMember pawnMember)
            {
                var character = pawnMember.Pawn;
                memberString = $"{member.MemberIndex} : {character.Name}";
            }
            responses.Add(ChatResponse.ServerChat(client, memberString));
        }
    }
}

return new ChatCommand();
