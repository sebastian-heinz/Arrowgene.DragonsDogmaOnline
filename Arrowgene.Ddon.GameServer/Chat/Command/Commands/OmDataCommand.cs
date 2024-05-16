using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using MySqlConnector;

namespace Arrowgene.Ddon.GameServer.Chat.Command.Commands
{
    public class OmDataCommand : ChatCommand
    {
        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "omdata";
        public override string HelpText => "usage: `/omdata [stageId?]` - Print current OM Data values";

        private DdonGameServer _server;

        public OmDataCommand(DdonGameServer server)
        {
            _server = server;
        }

        protected class OmFields
        {
            public static readonly Bitfield StageId = new Bitfield(15, 4, "StageId"); 
            public static readonly Bitfield GroupId = new Bitfield(22, 16, "GroupId");
            public static readonly Bitfield Unk0 = new Bitfield(27, 23, "Unk0");
            public static readonly Bitfield Increment = new Bitfield(60, 28, "Increment"); // bit 61 appears to be reserved for something
        }

        public override void Execute(string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
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
                    responses.Add(new ChatResponse() { Message = $"StageId={stageId} has no OM data" });
                    return;
                }

                foreach (var datum in client.Party.InstanceOmData[stageId])
                {
                    responses.Add(new ChatResponse() { Message = $"key=0x{datum.Key:x16}, value=0x{datum.Value:x8}"});

                    ulong eStageId = OmFields.StageId.Get(datum.Key);
                    ulong eGroupId = OmFields.GroupId.Get(datum.Key);
                    responses.Add(new ChatResponse() { Message = $"  StageId={eStageId}, GroupId={eGroupId}" });
                }
            }
        }
    }
}
