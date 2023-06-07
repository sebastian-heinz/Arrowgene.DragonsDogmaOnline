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
    public class SkillLearnSkillHandler : GameStructurePacketHandler<C2SSkillLearnSkillReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillLearnSkillHandler));
        
        public SkillLearnSkillHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillLearnSkillReq> packet)
        {
            CustomSkill newSkill = new CustomSkill()
            {
                Job = packet.Structure.Job,
                SkillId = packet.Structure.SkillId,
                SkillLv = packet.Structure.SkillLv
            };
            client.Character.LearnedCustomSkills.Add(newSkill);
            Server.Database.ReplaceLearnedCustomSkill(client.Character.CommonId, newSkill);

            // TODO: substract cost, save to db

            client.Send(new S2CSkillLearnSkillRes()
            {
                Job = packet.Structure.Job,
                NewJobPoint = client.Character.ActiveCharacterJobData.JobPoint,
                SkillId = packet.Structure.SkillId,
                SkillLv = packet.Structure.SkillLv
            });
        }
    }
}