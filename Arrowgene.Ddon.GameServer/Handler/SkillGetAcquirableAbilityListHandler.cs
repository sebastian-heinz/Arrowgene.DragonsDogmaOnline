using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetAcquirableAbilityListHandler : GameRequestPacketHandler<C2SSkillGetAcquirableAbilityListReq, S2CSkillGetAcquirableAbilityListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetAcquirableAbilityListHandler));


        public static CDataAbilityParam GetAbilityFromId(uint abilityId)
        {
            var abilities = SkillData.AllAbilities.Concat(SkillData.AllSecretAbilities);
            return abilities.Where(x => x.AbilityNo == abilityId).FirstOrDefault();
        }


        public SkillGetAcquirableAbilityListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSkillGetAcquirableAbilityListRes Handle(GameClient client, C2SSkillGetAcquirableAbilityListReq request)
        {
            S2CSkillGetAcquirableAbilityListRes response = new S2CSkillGetAcquirableAbilityListRes();
            if (request.Job != 0)
            {
                response.AbilityParamList = (client.GameMode == GameMode.Normal) ?
                    client.Character.AcquirableAbilities[request.Job] :
                    SkillData.AllAbilities.Where(x => x.Job == request.Job).ToList();
            }
            else if (request.CharacterId == 0)
            {
                // Player characters come in as CharacterId == 0.
                // Pawns seem to not need the information from this query. The UI still is populated by the skills
                // acquired by the player character (is this intended?).
                List<AbilityId> UnlockedAbilities = Server.Database.SelectAllUnlockedSecretAbilities(client.Character.CommonId);
                response.AbilityParamList = SkillData.AllSecretAbilities.Where(x => UnlockedAbilities.Contains((AbilityId)x.AbilityNo)).ToList();
            }

            return response;
        }
    }
}
