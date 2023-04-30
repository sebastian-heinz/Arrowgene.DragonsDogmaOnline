using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class SkillManager
    {
        public CustomSkill SetSkill(IDatabase database, GameClient client, CharacterCommon character, JobId job, byte slotNo, uint skillId, byte skillLv)
        {
            // TODO: Check in DB if the skill is unlocked and it's leveled up to what the packet says
            
            CustomSkill skillSlot = character.CustomSkills
                .Where(skill => skill.Job == job && skill.SlotNo == slotNo)
                .FirstOrDefault();
            
            if(skillSlot == null)
            {
                skillSlot = new CustomSkill()
                {
                    Job = job,
                    SlotNo = slotNo,
                    SkillId = skillId,
                    SkillLv = skillLv
                };
                character.CustomSkills.Add(skillSlot);
            }
            else
            {
                skillSlot.SkillId = skillId;
                skillSlot.SkillLv = skillLv;
            }

            database.ReplaceEquippedCustomSkill(character.CommonId, skillSlot);

            // Inform party members of the change
            if(job == character.Job)
            {
                if(character is Character)
                {
                    client.Party.SendToAll(new S2CSkillCustomSkillSetNtc()
                    {
                        CharacterId = ((Character) character).CharacterId,
                        ContextAcquirementData = new CDataContextAcquirementData()
                        {
                            SlotNo = skillSlot.SlotNo,
                            AcquirementNo = skillSlot.SkillId,
                            AcquirementLv = skillSlot.SkillLv
                        }
                    });
                }
                else if(character is Pawn)
                {
                    client.Party.SendToAll(new S2CSkillPawnCustomSkillSetNtc()
                    {
                        PawnId = ((Pawn) character).PawnId,
                        ContextAcquirementData = new CDataContextAcquirementData()
                        {
                            SlotNo = skillSlot.SlotNo,
                            AcquirementNo = skillSlot.SkillId,
                            AcquirementLv = skillSlot.SkillLv
                        }
                    });
                }
            }

            return skillSlot;
        }
    }
}