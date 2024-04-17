using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillLearnPawnNormalSkillHandler : GameStructurePacketHandler<C2SSkillLearnPawnNormalSkillReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillLearnPawnNormalSkillHandler));

        private readonly JobManager _jobManager;

        public SkillLearnPawnNormalSkillHandler(DdonGameServer server) : base(server)
        {
            this._jobManager = server.JobManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillLearnPawnNormalSkillReq> packet)
        {
            Pawn Pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).Single();

            _jobManager.UnlockLearnedNormalSkill(Server.AssetRepository, Server.Database, client, Pawn,
                                                 packet.Structure.Job, packet.Structure.SkillId);
        }
    }
}
