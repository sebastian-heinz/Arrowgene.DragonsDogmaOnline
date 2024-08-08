using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillSetPawnAbilityHandler : GameRequestPacketHandler<C2SSkillSetPawnAbilityReq, S2CSkillSetPawnAbilityRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillSetPawnAbilityHandler));
        
        private readonly JobManager jobManager;

        public SkillSetPawnAbilityHandler(DdonGameServer server) : base(server)
        {
            jobManager = server.JobManager;
        }

        public override S2CSkillSetPawnAbilityRes Handle(GameClient client, C2SSkillSetPawnAbilityReq packet)
        {
            if(packet.SlotNo == 0)
            {
                Logger.Error(client, $"Requesting to set an ability to slot 0");
            }

            // For some reason JobId is received as 0, unlike in SkillSetAbilityHandler, where it's set to its correct value
            // This is, also for whatever reason, important so it works properly, so we have to set it ourselves
            // TODO: Investigate this more, or optimize this

            var AllAbilities = SkillGetAcquirableAbilityListHandler.AllAbilities.Concat(SkillGetAcquirableAbilityListHandler.AllSecretAbilities);
            JobId abilityJob = AllAbilities
                .Where(aug => aug.AbilityNo == packet.SkillId )
                .Select(aug => aug.Job)
                .Single();

            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.PawnId).Single();
            Ability abilitySlot = jobManager.SetAbility(Server.Database, client, pawn, abilityJob, packet.SlotNo, packet.SkillId, packet.SkillLv);

            return new S2CSkillSetPawnAbilityRes() {
                PawnId = pawn.PawnId,
                SlotNo = packet.SlotNo,
                AbilityId = abilitySlot.AbilityId,
                AbilityLv = abilitySlot.AbilityLv
            };
        }
    }
}
