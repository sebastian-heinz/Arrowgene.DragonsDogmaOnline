using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetCurrentSetSkillListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetCurrentSetSkillListHandler));

        public SkillGetCurrentSetSkillListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_SKILL_GET_CURRENT_SET_SKILL_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
             // TODO: Filter so only the current job skills are sent?
            S2CSkillGetCurrentSetSkillListRes res = new S2CSkillGetCurrentSetSkillListRes();
            res.NormalSkillList = client.Character.NormalSkills;
            res.SetCustomSkillList = client.Character.CustomSkills;
            res.SetAbilityList = client.Character.Abilities;
            client.Send(res);
        }
    }
}
