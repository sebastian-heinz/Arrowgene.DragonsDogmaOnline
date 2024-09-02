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
    public class JobOrbTreeGetJobOrbTreeStatusListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobOrbTreeGetJobOrbTreeStatusListHandler));

        public JobOrbTreeGetJobOrbTreeStatusListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_JOB_ORB_TREE_GET_JOB_ORB_TREE_STATUS_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            S2CJobOrbTreeGetJobOrbTreeStatusListRes Response = new S2CJobOrbTreeGetJobOrbTreeStatusListRes();

            // TODO: Currently all job menus are not released. In the future, these should
            // TODO: be restricted behind some conditions
            // CDataCharacterJobData CharacterJobData = client.Character.CharacterJobDataList.Where(cjd => cjd.Job == client.Character.Job).Single();
            foreach (var Job in (JobId[]) JobId.GetValues(typeof(JobId)))
            {
                Response.TreeStatusList.Add(new CDataJobOrbTreeStatus()
                {
                    JobId = Job,
                    IsReleased = false,
                    Rate = 0.0f
                });
            }

            client.Send(Response);
        }
    }
}
