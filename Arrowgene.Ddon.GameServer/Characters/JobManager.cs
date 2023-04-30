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

        public Ability SetAbility(IDatabase database, GameClient client, CharacterCommon character, JobId abilityJob, byte slotNo, uint abilityId, byte abilityLv)
        {
            // TODO: Check in DB if the skill is unlocked and it's leveled up to what the packet says
            
            Ability abilitySlot = character.Abilities
                .Where(ability => ability.EquippedToJob == character.Job && ability.SlotNo == slotNo)
                .FirstOrDefault();
            
            if(abilitySlot == null)
            {
                abilitySlot = new Ability()
                {
                    EquippedToJob = character.Job,
                    Job = abilityJob,
                    SlotNo = slotNo,
                };
                character.Abilities.Add(abilitySlot);
            }
            
            abilitySlot.Job = abilityJob;
            abilitySlot.AbilityId = abilityId;
            abilitySlot.AbilityLv = abilityLv;

            database.ReplaceEquippedAbility(character.CommonId, abilitySlot);

            // Inform party members of the change
            if(character is Character)
            {
                client.Party.SendToAll(new S2CSkillAbilitySetNtc()
                {
                    CharacterId = ((Character) character).CharacterId,
                    ContextAcquirementData = new CDataContextAcquirementData()
                    {
                        SlotNo = abilitySlot.SlotNo,
                        AcquirementNo = abilitySlot.AbilityId,
                        AcquirementLv = abilitySlot.AbilityLv
                    }
                });
            }
            else if(character is Pawn)
            {
                client.Party.SendToAll(new S2CSkillPawnAbilitySetNtc()
                {
                    PawnId = ((Pawn) character).PawnId,
                    ContextAcquirementData = new CDataContextAcquirementData()
                    {
                        SlotNo = abilitySlot.SlotNo,
                        AcquirementNo = abilitySlot.AbilityId,
                        AcquirementLv = abilitySlot.AbilityLv
                    }
                });
            }

            return abilitySlot;
        }

        public void RemoveAbility(IDatabase database, CharacterCommon character, byte slotNo)
        {
            // TODO: Performance
            List<Ability> newAbilities = new List<Ability>();
            lock(character.Abilities)
            {
                byte removedAbilitySlotNo = Byte.MaxValue;
                for(int i=0; i<character.Abilities.Count; i++)
                {
                    Ability ability = character.Abilities[i];
                    if(ability.EquippedToJob == character.Job && ability.SlotNo == slotNo)
                    {
                        character.Abilities.RemoveAt(i);
                        removedAbilitySlotNo = ability.SlotNo;
                        break;
                    }
                }

                for(int i=0; i<character.Abilities.Count; i++)
                {
                    Ability ability = character.Abilities[i];
                    if(ability.EquippedToJob == character.Job)
                    {
                        if(ability.SlotNo > removedAbilitySlotNo)
                        {
                            ability.SlotNo--;
                        }
                        newAbilities.Add(ability);
                    }
                }
            }

            database.ReplaceEquippedAbilities(character.CommonId, character.Job, newAbilities);

            // Same as skills, i haven't found an Ability off NTC. It may not be required
        }

        private uint GetBaseSkillId(uint skillId)
        {
            return skillId % 100;
        }
    }
}