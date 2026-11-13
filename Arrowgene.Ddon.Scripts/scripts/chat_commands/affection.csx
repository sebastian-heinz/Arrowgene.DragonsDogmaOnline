using Arrowgene.Ddon.GameServer.Handler;
using System.Collections.Generic;

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.User;
    public override string CommandName => "affection";
    public override string HelpText => "usage: `/affection` - Check pawn affection.";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        List<string> strings = [];
        foreach (var pawn in client.Character.Pawns)
        {
            int level = (int)pawn.PartnerPawnData.CalculateLikability();

            strings.Add($"{pawn.Name}{(client.Character.PartnerPawnId == pawn.PawnId ? " *" : " ")}");
            strings.Add($"    Affection: {level} / {PartnerPawnData.MaxLevel}");
            strings.Add($"    Progress: {pawn.PartnerPawnData.CalculateLikabilityXP()} / {PartnerPawnData.LikabilityCurve.ElementAtOrDefault(level+1)}");
            strings.Add($"    ");
        }

        client.Send(new S2CConnectionInformationNtc(strings));
    }
}

return new ChatCommand();
