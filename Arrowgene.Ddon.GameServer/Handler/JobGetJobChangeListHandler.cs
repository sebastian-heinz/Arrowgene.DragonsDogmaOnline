using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobGetJobChangeListHandler : GameRequestPacketHandler<C2SJobGetJobChangeListReq, S2CJobGetJobChangeListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobGetJobChangeListHandler));


        public JobGetJobChangeListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CJobGetJobChangeListRes Handle(GameClient client, C2SJobGetJobChangeListReq request)
        {
            bool isHighScepterUnlocked = client.Character.HasContentReleased(ContentsRelease.ChangetoHighScepter);
            if (client.GameMode == GameMode.BitterblackMaze)
            {
                isHighScepterUnlocked = true;
            }

            S2CJobGetJobChangeListRes res = new S2CJobGetJobChangeListRes();
            res.JobChangeInfo = buildJobChangeInfoList(client.Character, isHighScepterUnlocked);
            res.JobReleaseInfo = buildJobReleaseInfoList(client.Character, isHighScepterUnlocked);

            // TODO: Check for released jobs

            if (client.Party is not null)
            {
                var partyPawns = client.Party.Members.Where(x =>
                    x is PawnPartyMember pawnMember
                    && pawnMember.Pawn.PawnType == PawnType.Main
                    && pawnMember.Pawn.CharacterId == client.Character.CharacterId)
                    .Select(x => ((PawnPartyMember)x).Pawn)
                    .ToList();

                res.PawnJobChangeInfoList = client.Character.Pawns
                    .Where(x => partyPawns.Contains(x))
                    .Select((pawn, index) => new CDataPawnJobChangeInfo()
                    {
                        SlotNo = (byte)(index + 1),
                        PawnId = pawn.PawnId,
                        JobChangeInfoList = buildJobChangeInfoList(pawn, isHighScepterUnlocked),
                        JobReleaseInfoList = buildJobReleaseInfoList(pawn, isHighScepterUnlocked)
                    })
                    .ToList();
            }
            res.PlayPointList = client.Character.PlayPointList;

            return res;
        }

        private List<CDataJobChangeInfo> buildJobChangeInfoList(CharacterCommon common, bool isHighScepterUnlocked)
        {
            return common.CharacterJobDataList
                .Where(x => x.Job != JobId.HighScepter || isHighScepterUnlocked)
                .Select(jobData => getJobChangeInfo(common, jobData.Job))
                .ToList();
        }

        private List<CDataJobChangeInfo> buildJobReleaseInfoList(CharacterCommon common, bool isHighScepterUnlocked)
        {
            return ((JobId[]) JobId.GetValues(typeof(JobId)))
                .Where(x => x != JobId.HighScepter || isHighScepterUnlocked)
                .Where(x => !common.CharacterJobDataList.Any(job => job.Job == x))
                .Select(jobId => getJobChangeInfo(common, jobId))
                .ToList();
        }

        private CDataJobChangeInfo getJobChangeInfo(CharacterCommon common, JobId jobId)
        {
            return new CDataJobChangeInfo()
            {
                JobId = jobId,
                EquipItemList = common.EquipmentTemplate.EquipmentAsCDataEquipItemInfo(jobId, EquipType.Performance)
                    .Union(common.EquipmentTemplate.EquipmentAsCDataEquipItemInfo(jobId, EquipType.Visual))
                    .Where(x => x.ItemId > 0) // Strip empty slots.
                    .ToList()
            };
        }
    }
}
