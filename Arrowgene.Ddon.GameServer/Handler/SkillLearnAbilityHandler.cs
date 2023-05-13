using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillLearnAbilityHandler : GameStructurePacketHandler<C2SSkillLearnAbilityReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillLearnAbilityHandler));
        
        public SkillLearnAbilityHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillLearnAbilityReq> packet)
        {
            // TODO: Move to DB
            client.Character.LearnedAbilities.Add(new CDataLearnedSetAcquirementParam()
            {
                Job = packet.Structure.Job,
                Type = 0, // TODO: Figure out
                AcquirementNo = packet.Structure.AbilityId,
                AcquirementLv = packet.Structure.AbilityLv,
                AcquirementParamId = 0 // TODO: Figure out
            });
            client.Send(new S2CSkillLearnAbilityRes()
            {
                Job = packet.Structure.Job,
                NewJobPoint = client.Character.ActiveCharacterJobData.JobPoint, // TODO: substract cost, save to db
                AbilityId = packet.Structure.AbilityId,
                AbilityLv = packet.Structure.AbilityLv
            });
        }
    }
}