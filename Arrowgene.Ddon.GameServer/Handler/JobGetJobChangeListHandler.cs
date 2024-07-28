using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobGetJobChangeListHandler : StructurePacketHandler<GameClient, C2SJobGetJobChangeListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobGetJobChangeListHandler));


        public JobGetJobChangeListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SJobGetJobChangeListReq> packet)
        {
            S2CJobGetJobChangeListRes jobChangeList = new S2CJobGetJobChangeListRes();
            jobChangeList.JobChangeInfo = this.buildJobChangeInfoList(client.Character);
            jobChangeList.JobReleaseInfo = this.buildJobReleaseInfoList(client.Character);
            jobChangeList.PawnJobChangeInfoList = client.Character.Pawns
                .Select((pawn, index) => new CDataPawnJobChangeInfo()
                {
                    SlotNo = (byte) (index+1),
                    PawnId = pawn.PawnId,
                    JobChangeInfoList = this.buildJobChangeInfoList(pawn),
                    JobReleaseInfoList = this.buildJobReleaseInfoList(pawn)
                })
                .ToList();
            jobChangeList.PlayPointList = client.Character.PlayPointList;
            client.Send(jobChangeList);
        }

        private List<CDataJobChangeInfo> buildJobChangeInfoList(CharacterCommon common)
        {
            return common.CharacterJobDataList
                .Select(jobData => this.getJobChangeInfo(common, jobData.Job))
                .ToList();
        }

        private List<CDataJobChangeInfo> buildJobReleaseInfoList(CharacterCommon common)
        {
            return ((JobId[]) JobId.GetValues(typeof(JobId)))
                .Select(jobId => this.getJobChangeInfo(common, jobId))
                .ToList();
        }

        private CDataJobChangeInfo getJobChangeInfo(CharacterCommon common, JobId jobId)
        {
            return new CDataJobChangeInfo()
            {
                JobId = jobId,
                EquipItemList = common.EquipmentTemplate.EquipmentAsCDataEquipItemInfo(jobId, EquipType.Performance)
                    .Union(common.EquipmentTemplate.EquipmentAsCDataEquipItemInfo(jobId, EquipType.Visual))
                    .ToList()
            };
        }
    }
}
