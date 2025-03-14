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
    public class SkillGetCurrentSetSkillListHandler : GameRequestPacketHandler<C2SSkillGetCurrentSetSkillListReq, S2CSkillGetCurrentSetSkillListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetCurrentSetSkillListHandler));

        public SkillGetCurrentSetSkillListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSkillGetCurrentSetSkillListRes Handle(GameClient client, C2SSkillGetCurrentSetSkillListReq request)
        {
            // TODO: Check if its necessary to filter so only the current job skills are sent
            S2CSkillGetCurrentSetSkillListRes res = new S2CSkillGetCurrentSetSkillListRes();
            res.NormalSkillList = client.Character.LearnedNormalSkills
                .Where(x => x.Job == client.Character.Job)
                .ToList();
            res.SetCustomSkillList = client.Character.EquippedCustomSkillsDictionary[client.Character.Job]
                .Select((x, index) => x?.AsCDataSetAcquirementParam((byte)(index+1)))
                .Where(x => x != null)
                .ToList();
            res.SetAbilityList = client.Character.EquippedAbilitiesDictionary[client.Character.Job]
                .Select((x, index) => x?.AsCDataSetAcquirementParam((byte)(index+1)))
                .Where(x => x != null)
                .ToList();
            return res;
        }
    }
}
