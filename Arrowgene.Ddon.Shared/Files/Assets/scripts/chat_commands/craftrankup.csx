public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName            => "craftrankup";
    public override string HelpText               => "/craftrankup pawn_name rank";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        if (command.Length < 2)
        {
            responses.Add(ChatResponse.CommandError(client, "Not enough parameters provided"));
            return;
        }

        var pawnName = command[0];
        var rank = uint.Parse(command[1]);

        var pawnData = client.Character.Pawns
            .Select((pawn, index) => new { pawn = pawn, pawnNumber = (byte)(index + 1) })
            .FirstOrDefault(tuple => tuple.pawn.Name == pawnName);

		var currentRank = pawnData.pawn.CraftData.CraftRank;
        if (currentRank > rank)
        {
            return;
        }

		var expBands = LibDdon.GetSetting<List<uint>>("Crafting", "CraftRankExpLimit");
		
		uint expTotal = expBands[(int)rank] - pawnData.pawn.CraftData.CraftExp;

        server.CraftManager.HandlePawnExpUpNtc(client, pawnData.pawn, expTotal, 0);
		if (server.CraftManager.CanPawnRankUp(pawnData.pawn))
		{
			server.CraftManager.HandlePawnRankUpNtc(client, pawnData.pawn);
		}
    }
}

return new ChatCommand();