public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName            => "omdata";
    public override string HelpText               => "usage: `/omdata [stageId?]` - Print current OM Data values";

    private class OmFields
    {
        public static readonly Bitfield StageId = new Bitfield(15, 4, "StageId"); 
        public static readonly Bitfield GroupId = new Bitfield(22, 16, "GroupId");
        public static readonly Bitfield Unk0 = new Bitfield(27, 23, "Unk0");
        public static readonly Bitfield Increment = new Bitfield(60, 28, "Increment"); // bit 61 appears to be reserved for something
    }

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        uint stageId = client.Character.Stage.Id;
        if (command.Length >= 1)
        {
            if (UInt32.TryParse(command[0], out uint parsedAmount))
            {
                stageId = parsedAmount;
            }
            else
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid StageId \"{command[0]}\". It must be a number"));
                return;
            }
        }

        lock(client.Party.InstanceOmData)
        {
            if (!client.Party.InstanceOmData.ContainsKey(stageId))
            {
                responses.Add(ChatResponse.ServerChat(client, $"StageId={stageId} has no OM data"));
                return;
            }

            foreach (var datum in client.Party.InstanceOmData[stageId])
            {
                responses.Add(ChatResponse.ServerChat(client, $"key=0x{datum.Key:x16}, value=0x{datum.Value:x8}"));

                ulong eStageId = OmFields.StageId.Get(datum.Key);
                ulong eGroupId = OmFields.GroupId.Get(datum.Key);
                responses.Add(ChatResponse.ServerChat(client, $"  StageId={eStageId}, GroupId={eGroupId}"));
            }
        }
    }
}

return new ChatCommand();
