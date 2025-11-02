#load "libs.csx"

public class ChatCommand : IChatCommand
{
    private static readonly ILogger Logger = LogProvider.Logger(typeof(ChatCommand));

    public override AccountStateType AccountState => AccountStateType.User;
    public override string CommandName => "gc";
    public override string HelpText => "usage: `/gc help/list/join/create [*]` - Commands for Group Chat channels.";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        if (!client.Character.HasContentReleased(ContentsRelease.MainMenu))
        {
            responses.Add(ChatResponse.CommandError(client, "You do not have this feature unlocked yet."));
            return;
        }

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
                                "/gc help: Open this help window.",
                                "/gc list: List all available group chat channels.",
                                "/gc join [name]: Join a group chat channel.",
                                "/gc create [name] [desc]: Create a group chat channel."
                            ]));
                    break;
                }
            case "list":
                {
                    var groups = server.Database.SelectGroupChatGroups();

                    if (groups.Count > 0)
                    {
                        client.Send(new S2CConnectionInformationNtc(groups.OrderBy(x => x.Key).Select(x => $"{x.Key} : {x.Value.Desc} ({x.Value.Count}/{x.Value.CountTotal})")));
                    }
                    else
                    {
                        responses.Add(ChatResponse.CommandError(client, $"There are currently no open group chats."));
                    }

                    break;
                }
            case "join":
                {
                    if (command.Length == 1)
                    {
                        responses.Add(ChatResponse.CommandError(client, $"Insufficient arguments to subcommand."));
                    }

                    var groupName = command[1].ToLowerInvariant();
                    PacketQueue queue = new();
                    server.Database.ExecuteInTransaction(connection =>
                    {
                        if (server.GroupChatManager.JoinGroupChatByName(client, groupName, out var joinQueue, connection))
                        {
                            queue.AddRange(joinQueue);
                        }
                        else
                        {
                            responses.Add(ChatResponse.CommandError(client, $"\"{groupName}\" is not an existing group chat name."));
                        }
                    });

                    queue.Send();
  
                    break;
                }
            case "create":
                {
                    if (command.Length == 1)
                    {
                        responses.Add(ChatResponse.CommandError(client, $"Insufficient arguments to subcommand."));
                    }

                    var groupName = command[1].ToLowerInvariant();
                    var groupDesc = command.Length > 2 ? string.Join(" ", command[2..]) : string.Empty;

                    PacketQueue queue = new();

                    server.Database.ExecuteInTransaction(connection =>
                    {
                        if (server.GroupChatManager.CreateGroupChat(client, groupName, groupDesc, out var createQueue, connection))
                        {
                            queue.AddRange(createQueue);
                        }
                        else
                        {
                            responses.Add(ChatResponse.CommandError(client, $"\"{groupName}\" could not be created; it may already exist."));
                        }
                    });

                    queue.Send();

                    break;
                }
            default:
                {
                    responses.Add(ChatResponse.CommandError(client, $"Unknown subcommand."));
                    break;
                }
        }
    }
}

return new ChatCommand();
