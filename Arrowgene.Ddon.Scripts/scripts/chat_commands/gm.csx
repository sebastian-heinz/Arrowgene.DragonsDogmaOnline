#load "libs.csx"

public class ChatCommand : IChatCommand
{
    private static readonly ILogger Logger = LogProvider.Logger(typeof(ChatCommand));

    public override AccountStateType AccountState => AccountStateType.GameMaster;
    public override string CommandName => "gm";
    public override string HelpText => "usage: `/gm list/kick/mute/unmute/ban [*]` - Commands for Game Masters.";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        
        string chosenCommand = "help";
        if (command.Length >= 1)
        {
            chosenCommand = command[0];
        }

        switch (chosenCommand.ToLowerInvariant())
        {
            case "help":
                {
                    client.Send(new S2CConnectionInformationNtc([
                        $"/gm help: Print this.",
                        $"/gm list: See the current online player details.",
                        $"/gm kick [accountId]: Kick a player.",
                        $"/gm mute [accountId]: Mute a player.",
                        $"/gm unmute [accountId]: Unmute a player.",
                        $"/gm ban [accountId]: Ban a player."
                    ]));

                    break;
                }
            case "list":
                {
                    client.Send(new S2CConnectionInformationNtc()
                    {
                        ParagraphList = [.. server.ClientLookup.GetAll().OrderBy(x => x.Account.Id).Select((x, i) => new CDataInformationParagraph()
                        {
                            Index = (uint)i,
                            Text = $@"{x.Character?.CDataCharacterName.ToString() ?? "???"}
  cID {x.Character?.CharacterId.ToString() ?? "-"} / aID {x.Account.Id} ({x.Account.State})",
                        })]
                    });
                    break;
                }
            case "kick":
                {
                    if (GetClient(server, client, 1, command, responses, out var target) && target.Account.State < client.Account.State)
                    {
                        target.Send(new S2CConnectionErrorNtc() { ErrorCode = ErrorCode.ERROR_CODE_GM_KICK });
                        target.Close();
                    }
                    else
                    {
                        responses.Add(ChatResponse.CommandError(client, "The command was not executed."));
                    }
                    break;
                }
            case "mute":
                {
                    if (GetClient(server, client, 1, command, responses, out var target) && target.Account.State < client.Account.State)
                    {
                        target.Account.State = AccountStateType.Muted;
                        server.Database.UpdateAccount(target.Account);
                    }
                    else
                    {
                        responses.Add(ChatResponse.CommandError(client, "The command was not executed."));
                    }
                    break;
                }
            case "unmute":
                {
                    if (GetClient(server, client, 1, command, responses, out var target) && target.Account.State == AccountStateType.Muted)
                    {
                        target.Account.State = AccountStateType.User;
                        server.Database.UpdateAccount(target.Account);
                    }
                    else
                    {
                        responses.Add(ChatResponse.CommandError(client, "The command was not executed."));
                    }
                    break;
                }
            case "ban":
                {
                    if (GetClient(server, client, 1, command, responses, out var target) && target.Account.State < client.Account.State)
                    {
                        target.Account.State = AccountStateType.Banned;
                        server.Database.UpdateAccount(target.Account);
                        server.Database.DeleteTokenByAccountId(target.Account.Id);
                        target.Send(new S2CConnectionErrorNtc() { ErrorCode = ErrorCode.ERROR_CODE_GM_BAN });
                        target.Close();
                    }
                    else
                    {
                        responses.Add(ChatResponse.CommandError(client, "The command was not executed."));
                    }
                    break;
                }
            default:
                {
                    responses.Add(ChatResponse.CommandError(client, $"Unknown subcommand."));
                    break;
                }
        }
    }

    private bool GetClient(DdonGameServer server, GameClient client, int index, string[] command, List<ChatResponse> responses, out GameClient target)
    {
        if (index < command.Length && int.TryParse(command[index], out var accId))
        {
            target = server.ClientLookup.GetClientByAccountId(accId);

            if (target is null)
            {
                responses.Add(ChatResponse.CommandError(client, "No client was found by that ID."));
                return false;
            }

            return true;
        }
        else
        {
            responses.Add(ChatResponse.CommandError(client, "No valid account ID was provided."));
            target = null;
            return false;
        }
    }
}

return new ChatCommand();
