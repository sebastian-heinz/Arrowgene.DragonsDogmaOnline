using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
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

            //TODO: Figure out how to send a totally fake quest to the client at the right time so we don't need to use a quest file.
            //An actual questId is needed for the client to actually accept the progress update.
            //I assume we could slip something in whenever it asks for the quest list, but this works for now.
            //Or, refactor after the BBI PR to use S2C_BATTLE_71_12_16_NTC;
            QuestId baseId = (QuestId)70000001;
            var baseQuest = QuestManager.GetQuest(baseId);
            if (baseQuest is null)
            {
                responses.Add(ChatResponse.CommandError(client, $"Missing base quest."));
                return;
            }
            var questResultCommands = new List<CDataQuestCommand>()
            {
                QuestManager.ResultCommand.StageJump((StageNo) stageNo, startingLocation)
            };

            //Send fake progress update to trigger the warp command.
            S2CQuestQuestProgressNtc progressNtc = new S2CQuestQuestProgressNtc()
            {
                ProgressCharacterId = client.Character.CharacterId,
                QuestScheduleId = (uint)baseId,
                QuestProcessStateList = new List<CDataQuestProcessState>()
                {
                    new CDataQuestProcessState()
                    {
                        ProcessNo = 0,
                        SequenceNo = 0,
                        BlockNo = 2,
                        ResultCommandList = questResultCommands
                    }
                }
            };
            client.Send(progressNtc);

            //Reset quest progress so you can warp again.
            S2CQuestQuestProgressNtc resetNtc = new S2CQuestQuestProgressNtc()
            {
                ProgressCharacterId = client.Character.CharacterId,
                QuestScheduleId = (uint)baseId,
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
