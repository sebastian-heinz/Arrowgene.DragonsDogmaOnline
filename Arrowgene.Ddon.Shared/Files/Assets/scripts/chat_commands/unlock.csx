using System.Collections.Generic;
using System;

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName => "unlock";
    public override string HelpText => "usage: `/unlock groupId [areaBoss?]` - Send a defeat announcement for a group.";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        if (command.Length == 0)
        {
            // check expected length before accessing
            responses.Add(ChatResponse.CommandError(client, "No arguments provided."));
            return;
        }

        uint parsedId = client.Character.Stage.GroupId;
        if (command.Length >= 1)
        {
            if (Int32.TryParse(command[0], out int groupId))
            {
                parsedId = (uint)groupId;
            }
            else
            {
                responses.Add(ChatResponse.CommandError(client, $"Error: Invalid groupId"));
                return;
            }
        }

        bool isAreaBoss = false;
        if (command.Length >= 2)
        {
            if (bool.TryParse(command[1], out bool parsedBoss))
            {
                isAreaBoss = parsedBoss;
            }
            else
            {
                responses.Add(ChatResponse.CommandError(client, $"Error: Invalid isAreaBoss"));
                return;
            }
        }

        responses.Add(ChatResponse.ServerMessage(client, $"Unlocking ({client.Character.Stage.Id}, 0, {parsedId})"));
        S2CInstanceEnemyGroupDestroyNtc groupDestroyedNtc = new S2CInstanceEnemyGroupDestroyNtc()
        {
            LayoutId = new CDataStageLayoutId(client.Character.Stage.Id, 0, parsedId),
            IsAreaBoss = isAreaBoss
        };
        client.Send(groupDestroyedNtc);
    }
}

return new ChatCommand();
