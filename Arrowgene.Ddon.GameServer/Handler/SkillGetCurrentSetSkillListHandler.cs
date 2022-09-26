using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetCurrentSetSkillListHandler : StructurePacketHandler<GameClient, C2SSkillGetCurrentSetSkillListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetCurrentSetSkillListHandler));

        public SkillGetCurrentSetSkillListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillGetCurrentSetSkillListReq> packet)
        {
             // TODO: Filter so only the current job skills are sent? Not sure
            S2CSkillGetCurrentSetSkillListRes res = new S2CSkillGetCurrentSetSkillListRes();
            res.NormalSkillList = client.Character.NormalSkills;
            res.SetCustomSkillList = client.Character.CustomSkills.Where(x => x.Job == client.Character.Job).ToList(); // Doesn't seem to be necessary for skills
            res.SetAbilityList = client.Character.Abilities.Where(x => x.Job == client.Character.Job).ToList(); // But it seems so for abilities
            client.Send(res);
        }
    }
}
