using System.Linq;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared;
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
            jobChangeList.JobChangeInfo = client.Character.CharacterEquipItemListDictionary
                .Union(client.Character.CharacterEquipViewItemListDictionary)
                .GroupBy(x => x.Key)
                .Select(x => new CDataJobChangeInfo() {
                    JobId = x.Key,
                    EquipItemList = x.SelectMany(x => x.Value).Select(x => x.AsCDataEquipItemInfo()).ToList() // Flatten group by
                })
                .ToList();
            jobChangeList.JobReleaseInfo = jobChangeList.JobReleaseInfo;
            // TODO: jobChangeList.PawnJobChangeInfoList
            jobChangeList.PlayPointList = client.Character.PlayPointList;
            client.Send(jobChangeList);
        }
    }
}
