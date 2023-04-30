using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class JobManager
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

        public IEnumerable<CustomSkill> ChangeExSkill(IDatabase database, GameClient client, CharacterCommon character, JobId job, uint skillId)
        {
            IEnumerable<CustomSkill> modifiedSkillSlots = character.CustomSkills
                .Where(skill => skill.Job == job && GetBaseSkillId(skill.SkillId) == GetBaseSkillId(skillId));
            foreach (CustomSkill skillSlot in modifiedSkillSlots)
            {
                skillSlot.SkillId = skillId;
                skillSlot.SkillLv = 1; // Must be 1 otherwise they do 0 damage
                SetSkill(database, client, character, skillSlot.Job, skillSlot.SlotNo, skillSlot.SkillId, skillSlot.SkillLv);
            }
            return modifiedSkillSlots;
        }

        public void RemoveSkill(IDatabase database, CharacterCommon character, JobId job, byte slotNo)
        {
            character.CustomSkills.RemoveAll(skill => skill.Job == job && skill.SlotNo == slotNo);

            // TODO: Error handling
            database.DeleteEquippedCustomSkill(character.CommonId, job, slotNo);

            // I haven't found a packet to notify this to other players
            // From what I tested it doesn't seem to be necessary
        }

        private uint GetBaseSkillId(uint skillId)
        {
            return skillId % 100;
        }
    }
}