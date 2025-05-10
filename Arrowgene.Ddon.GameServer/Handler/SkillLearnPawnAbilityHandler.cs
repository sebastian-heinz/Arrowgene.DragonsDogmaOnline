using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillLearnPawnAbilityHandler : GameRequestPacketQueueHandler<C2SSkillLearnPawnAbilityReq, S2CSkillLearnPawnAbilityRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillLearnPawnAbilityHandler));
        
        public SkillLearnPawnAbilityHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SSkillLearnPawnAbilityReq request)
        {
            var packets = new PacketQueue();

            Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);

            var ability = SkillData.AllAbilities.Concat(SkillData.AllSecretAbilities)
                .Where(aug => aug.AbilityNo == request.AbilityId)
                .SingleOrDefault()
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_SKILL_INVALID_SKILL_ID);

            Server.Database.ExecuteInTransaction(connection =>
            {
                packets = Server.JobManager.UnlockAbility(client, pawn, ability.Job, request.AbilityId, request.AbilityLv, connection);
            });

            return packets;
        }
    }
}
