using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetPackageQuestListHandler : GameRequestPacketHandler<C2SQuestGetPackageQuestListReq, S2CQuestGetPackageQuestListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetPackageQuestListHandler));

        public QuestGetPackageQuestListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetPackageQuestListRes Handle(GameClient client, C2SQuestGetPackageQuestListReq request)
        {
            var substorySequenceSettings = Server.GameSettings.Get<Dictionary<QuestSubstoryGroupId,List<uint>>>("substory", "SubstorySequence") ??
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_SERVER_CONFIG_ERROR);

            var substoryMissionMap = Server.GameSettings.Get<Dictionary<QuestSubstoryGroupId, Dictionary<uint, List<QuestId>>>>("substory", "SubstoryMissionMap") ??
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_SERVER_CONFIG_ERROR);

            var result = new S2CQuestGetPackageQuestListRes()
            {
                StageNo = request.StageNo
            };

            if (!client.Character.HasContentReleased(ContentsRelease.CooperatorsoftheRoyalFamily))
            {
                return result;
            }

            Dictionary<QuestSubstoryGroupId, Dictionary<uint, List<CDataQuestList>>> packageQuestGroups = new();
            foreach (var questScheduleId in QuestManager.GetQuestByStageNo(QuestType.Substory, request.StageNo))
            {
                var quest = QuestManager.GetQuestByScheduleId(questScheduleId);
                if (quest == null || client.Character.HasQuestCompleted(quest.QuestId))
                {
                    continue;
                }

                var substoryProperties = QuestManager.GetSubstoryQuestProperties(Server, quest.QuestId);
                if (substoryProperties.SubstoryGroupId == QuestSubstoryGroupId.Invalid)
                {
                    Logger.Error($"{quest.QuestId} ({quest.QuestScheduleId}) is of type Substory, but is not assigned to a substory mission");
                    continue;
                }

                int sequenceStep = client.Character.SubstoryProgress.TryGetValue(substoryProperties.SubstoryGroupId, out var sp) ? sp.SequenceStep : 0;
                var sequence = substorySequenceSettings[substoryProperties.SubstoryGroupId];
                if (sequenceStep >= sequence.Count)
                {
                    continue;
                }
                var seqNo = sequence[sequenceStep];

                // Check to see if this quest is part of the current sequence
                var sequenceQuests = substoryMissionMap[substoryProperties.SubstoryGroupId][seqNo];
                if (!sequenceQuests.Contains(quest.QuestId))
                {
                    // It's not, so just ignore it
                    continue;
                }

                // Check to see if the quests previous to this have been completed
                bool procedeWithSubstoryQuest = true;
                foreach (var questId in sequenceQuests)
                {
                    if (!client.Character.HasQuestCompleted(questId))
                    {
                        if (questId != quest.QuestId)
                        {
                            procedeWithSubstoryQuest = false;
                        }
                        break;
                    }
                }

                if (!procedeWithSubstoryQuest)
                {
                    continue;
                }

                var questStateManager = QuestManager.GetQuestStateManager(client, quest);
                if (questStateManager == null)
                {
                    continue;
                }

                uint step = 0;
                if (questStateManager.IsQuestActive(quest.QuestId))
                {
                    var state = questStateManager.GetQuestState(quest);
                    step = state?.Step ?? 0;
                }

                if (!packageQuestGroups.ContainsKey(substoryProperties.SubstoryGroupId))
                {
                    packageQuestGroups[substoryProperties.SubstoryGroupId] = new();
                }

                if (!packageQuestGroups[substoryProperties.SubstoryGroupId].ContainsKey(seqNo))
                {
                    packageQuestGroups[substoryProperties.SubstoryGroupId][seqNo] = new();
                }

                packageQuestGroups[substoryProperties.SubstoryGroupId][seqNo].Add(quest.ToCDataQuestList(step));
            }

            foreach (var (substoryGroupId, data) in packageQuestGroups)
            {
                var packageQuestList = new CDataPackageQuestList();
                packageQuestList.SubstoryGroupId = substoryGroupId;
                foreach (var (seqNo, questList) in data)
                {
                    packageQuestList.Details.Add(new()
                    {
                        SeqNo = seqNo,
                        CurrentProgress = 0,
                        MaxProgress = 0,
                        Unk3 = [],
                        Unk4 = true,
                        Unk5 = true,
                        QuestList = questList
                    });
                }
                result.PackageQuestList.Add(packageQuestList);
            }

            return result;
        }
    }
}
