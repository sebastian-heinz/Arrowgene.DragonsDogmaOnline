using System.Linq;
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
        
        public SkillLearnPawnNormalSkillHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillLearnPawnNormalSkillReq> packet)
        {
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).Single();
            CDataCharacterJobData characterJobData = pawn.CharacterJobDataList.Where(cjd => cjd.Job == packet.Structure.Job).Single();

            pawn.LearnedNormalSkills.Add(new CDataNormalSkillParam()
            {
                Job = packet.Structure.Job,
                SkillNo = packet.Structure.SkillId,
                Index = 1
            });
            // TODO: DB and substract JP
            client.Send(new S2CSkillLearnPawnNormalSkillRes()
            {
                PawnId = pawn.PawnId,
                Job = packet.Structure.Job,
                SkillId = packet.Structure.SkillId,
                NewJobPoint = characterJobData.JobPoint
            });
        }
    }
}
