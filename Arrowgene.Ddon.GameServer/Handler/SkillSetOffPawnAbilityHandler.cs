using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillSetOffPawnAbilityHandler : GameRequestPacketHandler<C2SSkillSetOffPawnAbilityReq, S2CSkillSetOffPawnAbilityRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillSetOffPawnAbilityHandler));
        
        private readonly JobManager jobManager;

        public SkillSetOffPawnAbilityHandler(DdonGameServer server) : base(server)
        {
            jobManager = server.JobManager;
        }

        public override S2CSkillSetOffPawnAbilityRes Handle(GameClient client, C2SSkillSetOffPawnAbilityReq request)
        {
            Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);
            jobManager.RemoveAbility(Server.Database, pawn, request.SlotNo);

            client.Send(new S2CSkillSetPresetPawnAbilityNtc()
            {
                PawnId = pawn.PawnId,
                AbilityDataList = pawn.EquippedAbilitiesDictionary[pawn.Job]
                .Where(x => x != null)
                .Select((x, i) => x.AsCDataContextAcquirementData((byte)(i + 1)))
                .ToList()
            });

            return new S2CSkillSetOffPawnAbilityRes()
            {
                PawnId = request.PawnId,
                SlotNo = request.SlotNo
            };

        }
    }
}
