using Arrowgene.Ddon.Shared.Model.EpitaphRoad;

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName            => "updateom";
    public override string HelpText               => "usage: `/updateom groupid posid value [range]`";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        if (command.Length < 3)
        {
            responses.Add(ChatResponse.CommandError(client, "not enough arguments provided"));
            return;
        }

        try
        {
            uint groupId = uint.Parse(command[0]);
            uint posId = uint.Parse(command[1]);
            byte value = byte.Parse(command[2]);
            uint limit = 0;

            bool spam = false;
            if (command.Length >= 4)
            {
                spam = true;
                limit = uint.Parse(command[3]);
            }

            if (!spam)
            {
                var ntc = new S2CSeasonDungeonSetOmStateNtc()
                {
                    LayoutId = new CDataStageLayoutId()
                    {
                        StageId = client.Character.Stage.Id,
                        GroupId = groupId
                    },
                    PosId = posId,
                    State = (SoulOrdealOmState)value
                };
                client.Party.SendToAll(ntc);
            }
            else
            {
                for (uint i = groupId; i < limit; i++)
                {
                    var ntc = new S2CSeasonDungeonSetOmStateNtc()
                    {
                        LayoutId = new CDataStageLayoutId()
                        {
                            StageId = client.Character.Stage.Id,
                            GroupId = i
                        },
                        PosId = posId,
                        State = (SoulOrdealOmState)value
                    };
                    client.Party.SendToAll(ntc);
                }
            }
        }
        catch (Exception)
        {
            responses.Add(ChatResponse.CommandError(client, "invalid arguments"));
        }
    }
}

return new ChatCommand();