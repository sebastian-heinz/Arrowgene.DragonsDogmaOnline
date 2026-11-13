using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetPackageQuestInfoDetailHandler : GameRequestPacketHandler<C2SQuestGetPackageQuestInfoDetailReq, S2CQuestGetPackageQuestInfoDetailRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetPackageQuestInfoDetailHandler));

        public QuestGetPackageQuestInfoDetailHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetPackageQuestInfoDetailRes Handle(GameClient client, C2SQuestGetPackageQuestInfoDetailReq req)
        {
            var substorySequenceSettings = Server.GameSettings.Get<Dictionary<QuestSubstoryGroupId, List<uint>>>("substory", "SubstorySequence") ??
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_SERVER_CONFIG_ERROR);

            var substoryMissionMap = Server.GameSettings.Get<Dictionary<QuestSubstoryGroupId, Dictionary<uint, List<QuestId>>>>("substory", "SubstoryMissionMap") ??
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_SERVER_CONFIG_ERROR);

            var result = new S2CQuestGetPackageQuestInfoDetailRes()
            {
                SubstoryGroupId = req.SubstoryGroupId
            };

            if (!substoryMissionMap.ContainsKey(req.SubstoryGroupId))
            {
                Logger.Error($"No Substory mapping exists for {req.SubstoryGroupId}");
                return result;
            }

            int step = client.Character.SubstoryProgress.TryGetValue(req.SubstoryGroupId, out var sp) ? sp.SequenceStep : 0;
            result.IsComplete = sp?.IsComplete ?? false;
            var sequence = substorySequenceSettings[req.SubstoryGroupId];
            if (step >= sequence.Count)
            {
                step = sequence.Count - 1;
            }

            for (var i = 0; i <= step; i++)
            {
                var seqNo = sequence[i];

                var packageQuestInfoDetail = new CDataPackageQuestInfoDetail()
                {
                    MissionId = seqNo
                };

                int completedQuests = 0;
                for (int j = 0; j < substoryMissionMap[req.SubstoryGroupId][seqNo].Count; j++)
                {
                    var quest = QuestManager.GetQuestByQuestId(substoryMissionMap[req.SubstoryGroupId][seqNo][j]);
                    if (quest == null)
                    {
                        Logger.Error($"The quest '{substoryMissionMap[req.SubstoryGroupId][seqNo][j]}' does not exist.");
                        break;
                    }

                    var isComplete = client.Character.HasQuestCompleted(quest.QuestId);
                    packageQuestInfoDetail.PackageQuestInfoDetailList.Add(new()
                    {
                        Unk0 = (uint) j,
                        QuestId = quest.QuestId,
                        Level = quest.BaseLevel,
                        QuestOrderConditionParamList = [],
                        ShowEntry = true,
                        DisplayQuestName = isComplete ? true : QuestManager.GetQuestStateManager(client, quest).IsQuestAccepted(quest.QuestScheduleId),
                        IsComplete = isComplete,
                    });

                    if (isComplete)
                    {
                        completedQuests += 1;
                    }
                    else
                    {
                        break;
                    }
                }

                packageQuestInfoDetail.CurrentProgress = completedQuests;
                packageQuestInfoDetail.MaxProgress = substoryMissionMap[req.SubstoryGroupId][seqNo].Count;
                packageQuestInfoDetail.Unk3 = true;
                packageQuestInfoDetail.Unk4 = true;
                packageQuestInfoDetail.IsComplete = packageQuestInfoDetail.CurrentProgress == packageQuestInfoDetail.MaxProgress;
                packageQuestInfoDetail.Unk6 = DateTimeOffset.FromUnixTimeSeconds(0);

                result.PackageQuestInfoDetailList.Add(packageQuestInfoDetail);
            }

            return result;
        }
    }
}
