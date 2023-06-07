using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
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
            Ability newAbility = new Ability()
            {
                Job = packet.Structure.Job,
                AbilityId = packet.Structure.AbilityId,
                AbilityLv = packet.Structure.AbilityLv
            };
            client.Character.LearnedAbilities.Add(newAbility);
            Server.Database.ReplaceLearnedAbility(client.Character.CommonId, newAbility);

            // TODO: substract cost, save to db

            client.Send(new S2CSkillLearnAbilityRes()
            {
                Job = packet.Structure.Job,
                NewJobPoint = client.Character.ActiveCharacterJobData.JobPoint,
                AbilityId = packet.Structure.AbilityId,
                AbilityLv = packet.Structure.AbilityLv
            });
        }
    }
}