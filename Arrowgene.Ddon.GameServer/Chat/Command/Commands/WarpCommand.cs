using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Quest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Chat.Command.Commands
{
    public class WarpCommand : ChatCommand
    {
        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "warp";
        public override string HelpText => "usage: `/warp [stageNo]` - Warp to a stage.";

        private DdonGameServer _server;

        public WarpCommand(DdonGameServer server)
        {
            _server = server;
        }

        public override void Execute(string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
        {
            int stageNo = 200;
            int startingLocation = 0;
            if (command.Length == 0)
            {
                // check expected length before accessing
                responses.Add(ChatResponse.CommandError(client, "No arguments provided."));
                return;
            }

            if (command.Length >= 1)
            {
                if (Int32.TryParse(command[0], out int parsedId))
                {
                    stageNo = parsedId;
                }
                else
                {
                    responses.Add(ChatResponse.CommandError(client, $"Invalid stageNo \"{command[0]}\". It must be a number"));
                    return;
                }
            }

            if (command.Length >= 2)
            {
                if (Int32.TryParse(command[1], out int parsedLoc))
                {
                    startingLocation = parsedLoc;
                }
                else
                {
                    responses.Add(ChatResponse.CommandError(client, $"Invalid starting location \"{command[1]}\". It must be a number"));
                    return;
                }
            }

            //TODO: Figure out how to send a totally fake quest to the client at the right time so we don't need to hijack a quest file.
            QuestId baseId = (QuestId)70000001;
            QuestAssetData baseAsset = _server.AssetRepository.QuestAssets.Quests.Where(x => x.QuestId == baseId).FirstOrDefault();
            if (baseAsset is null)
            {
                responses.Add(ChatResponse.CommandError(client, $"Missing base quest."));
                return;
            }

            var questCheckCommands = new List<CDataQuestProcessState.MtTypedArrayCDataQuestCommand>()
            {
                new CDataQuestProcessState.MtTypedArrayCDataQuestCommand()
                {
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        new CDataQuestCommand()
                        {
                            Command = (ushort)QuestCommandCheckType.StageNo,
                            Param01 = stageNo
                        }
                    },
                }
            };
            var questResultCommands = new List<CDataQuestCommand>()
            {
                new CDataQuestCommand()
                {
                    Command = (ushort)QuestResultCommand.StageJump,
                    Param01 = stageNo,
                    Param02 = startingLocation
                }
            };

            //Send fake progress update to trigger the warp command.
            S2CQuestQuestProgressNtc progressNtc = new S2CQuestQuestProgressNtc()
            {
                ProgressCharacterId = client.Character.CharacterId,
                QuestScheduleId = (uint)baseAsset.QuestScheduleId,
                QuestProcessStateList = new List<CDataQuestProcessState>()
                {
                    new CDataQuestProcessState()
                    {
                        ProcessNo = 0,
                        SequenceNo = 0,
                        BlockNo = 2,
                        ResultCommandList = questResultCommands,
                        CheckCommandList = questCheckCommands
                    }
                }
            };
            client.Send(progressNtc);

            //Reset quest progress so you can warp again.
            S2CQuestQuestProgressNtc resetNtc = new S2CQuestQuestProgressNtc()
            {
                ProgressCharacterId = client.Character.CharacterId,
                QuestScheduleId = (uint)baseAsset.QuestScheduleId,
                QuestProcessStateList = new List<CDataQuestProcessState>()
                {
                    new CDataQuestProcessState()
                    {
                        ProcessNo = 0,
                        SequenceNo = 0,
                        BlockNo = 1
                    }
                }
            };
            client.Send(resetNtc);
        }
    }
}
