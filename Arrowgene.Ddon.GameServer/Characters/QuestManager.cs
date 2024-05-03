using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Arrowgene.Ddon.GameServer.Characters.QuestManager;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class QuestManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestManager));
        private readonly QuestAsset _QuestAssets;
        public QuestManager(DdonGameServer server)
        {
            _QuestAssets = server.AssetRepository.QuestAsset;
        }

        public class LayoutFlag
        {
            public static CDataQuestLayoutFlagSetInfo Create(uint layoutFlag, StageNo stageNo, uint groupId)
            {
                return new CDataQuestLayoutFlagSetInfo()
                {
                    LayoutFlagNo = layoutFlag,
                    SetInfoList = new List<CDataQuestSetInfo>()
                    {
                        new CDataQuestSetInfo()
                        {
                            StageNo = (uint) stageNo,
                            GroupId = groupId
                        }
                    }
                };
            }
        }

        public static CDataMainQuest CloneMainQuest(QuestAsset questAssets, MainQuestId questId)
        {
            var template = JsonSerializer.Serialize(questAssets.MainQuests[(uint)questId]);
            return JsonSerializer.Deserialize<CDataMainQuest>(template);
        }

        public class AcceptConditions
        {
            public static CDataQuestOrderConditionParam NoRestriction()
            {
                return new CDataQuestOrderConditionParam() { Type = 0x0};
            }
            public static CDataQuestOrderConditionParam MinimumLevelRestriction(uint level)
            {
                return new CDataQuestOrderConditionParam() { Type = 0x1, Param01 = (int) level };
            }

            public static CDataQuestOrderConditionParam MinimumVocationRestriction(JobId jobId, uint level)
            {
                return new CDataQuestOrderConditionParam() { Type = 0x2, Param01 = (int)jobId, Param02 = (int) level};
            }

            public static CDataQuestOrderConditionParam Solo()
            {
                return new CDataQuestOrderConditionParam() { Type = 0x3};
            }

            public static CDataQuestOrderConditionParam MainQuestCompletionRestriction(MainQuestId questId)
            {
                return new CDataQuestOrderConditionParam() { Type = 0x6, Param01 = (int)questId };
            }
        }

        public class CheckCommand
        {
            public static List<CDataQuestProcessState.MtTypedArrayCDataQuestCommand> AddCheckCommands(List<CDataQuestCommand> commands)
            {
                List<CDataQuestProcessState.MtTypedArrayCDataQuestCommand> result = new List<CDataQuestProcessState.MtTypedArrayCDataQuestCommand>();

                // This struct seems to be a list serialized inside of a list
                result.Add(new CDataQuestProcessState.MtTypedArrayCDataQuestCommand());
                result[0].ResultCommandList = commands;

                return result;
            }

            public static List<CDataQuestProcessState.MtTypedArrayCDataQuestCommand> AppendCheckCommand(List<CDataQuestProcessState.MtTypedArrayCDataQuestCommand> obj, CDataQuestCommand command)
            {
                obj[0].ResultCommandList.Add(command);
                return obj;
            }

            public static List<CDataQuestProcessState.MtTypedArrayCDataQuestCommand> AppendCheckCommands(List<CDataQuestProcessState.MtTypedArrayCDataQuestCommand> obj, List<CDataQuestCommand> commands)
            {
                obj[0].ResultCommandList.Concat(commands);
                return obj;
            }

            /**
             * @brief Makes the "quest shield" appear over the NPCs head.
             * @param stageNo The stage number where the NPC is located.
             * @param npcId The ID of the NPC to speak with.
             * @param orderGroupSerial
             */
            public static CDataQuestCommand NpcTalkAndOrderUi(StageNo stageNo, NpcId npcId, int orderGroupSerial = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.NpcTalkAndOrderUi, Param01 = (int)stageNo, Param02 = (int)npcId, Param03 = orderGroupSerial };
            }

            /**
             * @brief
             * @param flagNo
             */
            public static CDataQuestCommand MyQstFlagOn(int flagNo)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.MyQstFlagOn, Param01 = (int)flagNo};
            }

            /**
             * @brief
             * @param stageNo
             */
            public static CDataQuestCommand StageNo(StageNo stageNo)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.StageNo, Param01 = (int)stageNo };
            }

            /**
             * @brief
             * @param stageNo The stage number to use in the check.
             */
            public static CDataQuestCommand StageNoWithoutMarker(StageNo stageNo)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.StageNoWithoutMarker, Param01 = (int)stageNo };
            }

            /**
             * @brief
             * @param flagNo
             */
            public static CDataQuestCommand MyQstFlagOff(int flagNo)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.MyQstFlagOff, Param01 = flagNo };
            }

            /**
             * @brief Triggers a cutscene when the player enters the stage.
             * @param stageNo The stage number.
             * @param sceNo The cutscene number.
             */
            public static CDataQuestCommand SceHitInWithoutMarker(StageNo stageNo, int sceNo)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.SceHitInWithoutMarker, Param01 = (int)stageNo, Param02 = sceNo };
            }

            /**
             * @brief
             * @param stageNo The stage number.
             * @param groupNo Corresponds to the enemy group in CDataMainQuest?
             * @param setNo
             * @param flagNo
             */
            public static CDataQuestCommand IsLinkageEnemyFlag(StageNo stageNo, int groupNo, int setNo, int flagNo)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsLinkageEnemyFlag, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = flagNo };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand DummyNotProgress()
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.DummyNotProgress };
            }

            /**
             * @brief
             * @param stageNo
             * @param eventNo
             */
            public static CDataQuestCommand EventEnd(StageNo stageNo, int eventNo)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.EventEnd, Param01 = (int) stageNo, Param02 = eventNo };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNp
             * @param setNo
             */
            public static CDataQuestCommand IsEnemyDead(StageNo stageNo, int groupNo, int setNo)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.DieEnemy, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo };
            }

            /**
             * @brief
             * @param stageNo
             * @param npcId
             */
            public static CDataQuestCommand TalkNpc(StageNo stageNo, NpcId npcId)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.TalkNpc, Param01 = (int)stageNo, Param02 = (int) npcId};
            }

            /**
             * @brief
             * @param stageNo
             * @param npcId
             */
            public static CDataQuestCommand TalkNpcWithoutMarker(StageNo stageNo, NpcId npcId)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.TalkNpcWithoutMarker, Param01 = (int)stageNo, };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             * @param questId
             */
            public static CDataQuestCommand NewTalkNpc(StageNo stageNo, int groupNo, int setNo, int questId)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.NewTalkNpc, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = questId };
            }

            /**
             * @brief
             * @param stageNo
             * @param sceNo
             */
            public static CDataQuestCommand SceHitIn(StageNo stageNo, int sceNo)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.SceHitIn, Param01 = (int)sceNo};
            }

            /**
             * @brief
             * @param stageNo
             */
            public static CDataQuestCommand IsStageNo(StageNo stageNo) 
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.StageNo, Param01 = (int)stageNo };
            }
        }

        public class ResultCommand
        {
            /**
             * @brief Makes the quest available in the NPC dialouge when selecting to speak with them.
             * @param npcId The ID of the NPC to speak with.
             * @param msgNo The message id of the text to show.
             */
            public static CDataQuestCommand QstTalkChg(NpcId npcId, int msgNo)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.QstTalkChg, Param01 = (int)npcId, Param02 = msgNo };
            }

            /**
             * @brief 
             * @param flagNo The flag number to check.
             */
            public static CDataQuestCommand QstLayoutFlagOn(int flagNo)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.QstLayoutFlagOn, Param01 = flagNo };
            }

            /**
             * @brief
             * @param flagNo
             * @param questId
             */
            public static CDataQuestCommand WorldManageLayoutFlagOn(int flagNo, int questId)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.WorldManageLayoutFlagOn, Param01 = flagNo, Param02 = questId };
            }

            /**
             * @brief
             * @param stageNo
             * @param eventNo
             * @param jumpStageNo
             * @param jumpStartPosNo
             */
            public static CDataQuestCommand EventExec(StageNo stageNo, int eventNo, StageNo jumpStageNo, int jumpStartPosNo)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.EventExec, Param01 = (int)stageNo, Param02 = eventNo, Param03 = (int) jumpStageNo, Param04 = jumpStartPosNo };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand SetCheckPoint()
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SetCheckPoint };
            }

            /**
             * @brief Sets the announce type for a quest.
             * @param announceType The type of message toast on the users screen.
             */
            public static CDataQuestCommand SetAnnounceType(QuestAnnounceType announceType)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SetAnnounce, Param01 = (int)announceType };
            }

            /**
             * @brief
             * @param flagNo
             */
            public static CDataQuestCommand MyQstFlagOn(int flagNo)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.MyQstFlagOn, Param01 = flagNo };
            }

            /**
             * @brief
             * @param flagNo
             */
            public static CDataQuestCommand MyQstFlagOff(int flagNo)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.MyQstFlagOff, Param01 = flagNo };
            }
        }
    }
}
