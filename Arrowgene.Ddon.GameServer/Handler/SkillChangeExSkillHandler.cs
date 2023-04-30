using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Linq;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.GameServer.Characters;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillChangeExSkillHandler : StructurePacketHandler<GameClient, C2SSkillChangeExSkillReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillChangeExSkillHandler));

        private readonly SkillManager skillManager;

        public SkillChangeExSkillHandler(DdonGameServer server) : base(server)
        {
            skillManager = server.SkillManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillChangeExSkillReq> packet)
        {
            // TODO: Apparently pawns EX skills can be set separately, but i dont know how because this packet doesnt send PawnID
            // https://www.youtube.com/watch?v=3rK6DtDJ8EE 1:05:00

            IEnumerable<CustomSkill> skillSlots = ChangeExSkills(client, client.Character, packet.Structure);
            foreach (Pawn pawn in client.Character.Pawns)
            {
                ChangeExSkills(client, pawn, packet.Structure);
            }

            client.Send(new S2CSkillChangeExSkillRes() {
                Job = packet.Structure.Job,
                SkillId = packet.Structure.SkillId,
                SkillLv = 1, // Must be 1 otherwise they do 0 damage
                Unk3 = packet.Structure.Unk0,
                SlotsToUpdate = skillSlots.Select(skill => new CDataCommonU8(skill.SlotNo)).ToList()
            });
        }

        private uint GetBaseSkillId(uint skillId)
        {
            return skillId % 100;
        }

        private IEnumerable<CustomSkill> ChangeExSkills(GameClient client, CharacterCommon common, C2SSkillChangeExSkillReq structure)
        {
            // Update all equiped skills of the type to the chosen EX skill
            IEnumerable<CustomSkill> skillSlots = client.Character.CustomSkills
                .Where(skill => skill.Job == structure.Job && GetBaseSkillId(skill.SkillId) == GetBaseSkillId(structure.SkillId));
            foreach (CustomSkill skillSlot in skillSlots)
            {
                skillSlot.SkillId = structure.SkillId;
                skillSlot.SkillLv = 1; // Must be 1 otherwise they do 0 damage
                skillManager.SetSkill(Server.Database, client, common, skillSlot.Job, skillSlot.SlotNo, skillSlot.SkillId, skillSlot.SkillLv);

                // Inform party members of the change
                if(structure.Job == client.Character.Job)
                {
                    client.Party.SendToAll(new S2CSkillCustomSkillSetNtc()
                    {
                        CharacterId = client.Character.CharacterId,
                        ContextAcquirementData = new CDataContextAcquirementData()
                        {
                            SlotNo = skillSlot.SlotNo,
                            AcquirementNo = skillSlot.SkillId,
                            AcquirementLv = skillSlot.SkillLv
                        }
                    });
                }
            }
            return skillSlots;
        }
    }
}