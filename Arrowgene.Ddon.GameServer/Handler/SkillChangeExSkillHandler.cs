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
            CharacterCommon character;
            if(packet.Structure.PawnId == 0)
            {
                character = client.Character;
            }
            else
            {
                character = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).Single();
            }

            IEnumerable<CustomSkill> skillSlots = client.Character.CustomSkills
                .Where(skill => skill.Job == packet.Structure.Job && GetBaseSkillId(skill.SkillId) == GetBaseSkillId(packet.Structure.SkillId));
            foreach (CustomSkill skillSlot in skillSlots)
            {
                skillSlot.SkillId = packet.Structure.SkillId;
                skillSlot.SkillLv = 1; // Must be 1 otherwise they do 0 damage
                skillManager.SetSkill(Server.Database, client, character, skillSlot.Job, skillSlot.SlotNo, skillSlot.SkillId, skillSlot.SkillLv);

                // Inform party members of the change
                if(packet.Structure.Job == client.Character.Job)
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

            client.Send(new S2CSkillChangeExSkillRes() {
                Job = packet.Structure.Job,
                SkillId = packet.Structure.SkillId,
                SkillLv = 1, // Must be 1 otherwise they do 0 damage
                PawnId = packet.Structure.PawnId,
                SlotsToUpdate = skillSlots.Select(skill => new CDataCommonU8(skill.SlotNo)).ToList()
            });
        }

        private uint GetBaseSkillId(uint skillId)
        {
            return skillId % 100;
        }
    }
}