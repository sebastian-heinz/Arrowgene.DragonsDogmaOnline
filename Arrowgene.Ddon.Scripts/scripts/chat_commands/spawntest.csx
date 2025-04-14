using Arrowgene.Ddon.GameServer.Enemies.Generators;

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName            => "spawntest";
    public override string HelpText               => "usage: `/spawntest` - Toggle spawning testing crystals.";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        SpawnTestingGenerator.ToggledOn = !SpawnTestingGenerator.ToggledOn;

        responses.Add(ChatResponse.ServerMessage(client, $"Spawn Testing Mode: {SpawnTestingGenerator.ToggledOn}"));
    }
}

return new ChatCommand();