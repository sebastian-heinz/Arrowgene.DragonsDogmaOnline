using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillLearnNormalSkillHandler : GameStructurePacketHandler<C2SSkillLearnNormalSkillReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillLearnNormalSkillHandler));
        
        public SkillLearnNormalSkillHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillLearnNormalSkillReq> packet)
        {
            CDataCharacterJobData characterJobData = client.Character.CharacterJobDataList.Where(cjd => cjd.Job == packet.Structure.Job).Single();

            client.Character.LearnedNormalSkills.Add(new CDataNormalSkillParam()
            {
                Job = packet.Structure.Job,
                SkillNo = packet.Structure.SkillId,
                Index = 4 // wtf
            });
            // TODO: DB and substract JP
            client.Send(new S2CSkillLearnNormalSkillRes()
            {
                Job = packet.Structure.Job,
                SkillId = packet.Structure.SkillId,
                NewJobPoint = characterJobData.JobPoint
            });
        }
    }
}