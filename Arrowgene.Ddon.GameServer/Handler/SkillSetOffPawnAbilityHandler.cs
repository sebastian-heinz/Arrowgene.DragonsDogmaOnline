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

        public override S2CSkillSetOffPawnAbilityRes Handle(GameClient client, C2SSkillSetOffPawnAbilityReq packet)
        {
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.PawnId).Single();
            jobManager.RemoveAbility(Server.Database, pawn, packet.SlotNo);

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
                PawnId = packet.PawnId,
                SlotNo = packet.SlotNo
            };

        }
    }
}
