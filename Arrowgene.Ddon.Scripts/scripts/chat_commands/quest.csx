#load "libs/ScriptUtils.csx"

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName => "quest";
    public override string HelpText => "usage: `/quest <reload>` - Performs operations related to quests";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        if (command.Length < 1)
        {
            responses.Add(ChatResponse.CommandError(client, "No arguments provided"));
            return;
        }

        try
        {
            var action = command[0].ToLower();

            switch (action)
            {
                case "reload":
                    client.Send(new S2CQuestMasterDataReloadNtc());
                    break;
                default:
                    responses.Add(ChatResponse.CommandError(client, $"Unknown action '{action}'."));
                    break;
            }
        }
        catch (Exception)
        {
            responses.Add(ChatResponse.CommandError(client, "Invalid Arguments"));
        }
    }
}

return new ChatCommand();
