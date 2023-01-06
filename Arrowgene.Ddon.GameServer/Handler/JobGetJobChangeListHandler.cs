using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobGetJobChangeListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobGetJobChangeListHandler));


        public JobGetJobChangeListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_JOB_GET_JOB_CHANGE_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            S2CJobGetJobChangeListRes jobChangeList = new S2CJobGetJobChangeListRes();
            jobChangeList.JobChangeInfo = client.Character.Equipment.getAllEquipment().Keys
                .Select(jobId => new CDataJobChangeInfo() {
                    JobId = jobId,
                    EquipItemList = client.Character.Equipment.getEquipmentAsCDataEquipItemInfo(jobId, EquipType.Performance)
                        .Union(client.Character.Equipment.getEquipmentAsCDataEquipItemInfo(jobId, EquipType.Visual))
                        .ToList()
                })
                .ToList();
            jobChangeList.JobReleaseInfo = jobChangeList.JobReleaseInfo;
            // TODO: jobChangeList.PawnJobChangeInfoList
            jobChangeList.PlayPointList = client.Character.PlayPointList;
            client.Send(jobChangeList);
        }
    }
}
