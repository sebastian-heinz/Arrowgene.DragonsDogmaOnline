using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillLearnPawnNormalSkillHandler : GameRequestPacketQueueHandler<C2SSkillLearnPawnNormalSkillReq, S2CSkillLearnPawnNormalSkillRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillLearnPawnNormalSkillHandler));

        private readonly JobManager _jobManager;

        public SkillLearnPawnNormalSkillHandler(DdonGameServer server) : base(server)
        {
            this._jobManager = server.JobManager;
        }

        public override PacketQueue Handle(GameClient client, C2SSkillLearnPawnNormalSkillReq request)
        {
            Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);

            return _jobManager.UnlockLearnedNormalSkill(Server.AssetRepository, Server.Database, client, pawn,
                                                 request.Job, request.SkillId);
        }
    }
}
