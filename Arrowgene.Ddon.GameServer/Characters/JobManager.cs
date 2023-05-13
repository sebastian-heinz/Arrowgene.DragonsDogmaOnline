using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class JobManager
    {
        public void SetJob(DdonServer<GameClient> server, GameClient client, CharacterCommon common, JobId jobId)
        {
            common.Job = jobId;

            server.Database.UpdateCharacterCommonBaseInfo(common);

            CDataCharacterJobData? activeCharacterJobData = common.ActiveCharacterJobData;

            if(activeCharacterJobData == null)
            {
                activeCharacterJobData = new CDataCharacterJobData();
                activeCharacterJobData.Job = jobId;
                activeCharacterJobData.Exp = 0;
                activeCharacterJobData.JobPoint = 0;
                activeCharacterJobData.Lv = 1;
                // TODO: All the other stats
                common.CharacterJobDataList.Add(activeCharacterJobData);
                server.Database.ReplaceCharacterJobData(common.CommonId, activeCharacterJobData);
            }

            // TODO: Figure out if it should send all equips or just the ones for the current job
            List<CDataEquipItemInfo> equipItemInfos = common.Equipment.getEquipmentAsCDataEquipItemInfo(common.Job, EquipType.Performance)
                .Union(common.Equipment.getEquipmentAsCDataEquipItemInfo(common.Job, EquipType.Visual))
                .ToList();
            List<CDataCharacterEquipInfo> characterEquipList = common.Equipment.getEquipmentAsCDataCharacterEquipInfo(common.Job, EquipType.Performance)
                .Union(common.Equipment.getEquipmentAsCDataCharacterEquipInfo(common.Job, EquipType.Visual))
                .ToList();

            List<CDataSetAcquirementParam> skills = common.CustomSkills
                .Where(x => x.Job == jobId)
                .Select(x => x.AsCDataSetAcquirementParam())
                .ToList();
            List<CDataSetAcquirementParam> abilities = common.Abilities
                .Where(x => x.EquippedToJob == jobId)
                .Select(x => x.AsCDataSetAcquirementParam())
                .ToList();
            List<CDataLearnNormalSkillParam> normalSkills = common.LearnedNormalSkills
                .Select(x => new CDataLearnNormalSkillParam(x))
                .ToList();
            List<CDataEquipJobItem> jobItems = common.CharacterEquipJobItemListDictionary[common.Job];

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            // TODO: Move previous job equipment to storage box, and move new job equipment from storage box

            if(common is Character)
            {
                Character character = (Character) common;

                S2CJobChangeJobNtc changeJobNotice = new S2CJobChangeJobNtc();
                changeJobNotice.CharacterId = character.CharacterId;
                changeJobNotice.CharacterJobData = activeCharacterJobData;
                changeJobNotice.EquipItemInfo = equipItemInfos;
                changeJobNotice.SetAcquirementParamList = skills;
                changeJobNotice.SetAbilityParamList = abilities;
                changeJobNotice.LearnNormalSkillParamList = normalSkills;
                changeJobNotice.EquipJobItemList = jobItems;
                // TODO: Unk0
                
                foreach(GameClient otherClient in server.ClientLookup.GetAll())
                {
                    otherClient.Send(changeJobNotice);
                }

                updateCharacterItemNtc.UpdateType = 0x28;
                client.Send(updateCharacterItemNtc);

                S2CJobChangeJobRes changeJobResponse = new S2CJobChangeJobRes();
                changeJobResponse.CharacterJobData = activeCharacterJobData;
                changeJobResponse.CharacterEquipList = characterEquipList;
                changeJobResponse.SetAcquirementParamList = skills;
                changeJobResponse.SetAbilityParamList = abilities;
                changeJobResponse.LearnNormalSkillParamList = normalSkills;
                changeJobResponse.EquipJobItemList = jobItems;
                changeJobResponse.PlayPointData = character.PlayPointList
                    .Where(x => x.Job == jobId)
                    .Select(x => x.PlayPoint)
                    .FirstOrDefault(new CDataPlayPointData());
                changeJobResponse.Unk0.Unk0 = (byte) jobId;
                changeJobResponse.Unk0.Unk1 = character.Storage.getAllStoragesAsCDataCharacterItemSlotInfoList();
            
                client.Send(changeJobResponse);
            }
            else if(common is Pawn)
            {
                Pawn pawn = (Pawn) common;

                S2CJobChangePawnJobNtc changeJobNotice = new S2CJobChangePawnJobNtc();
                changeJobNotice.CharacterId = pawn.CharacterId;
                changeJobNotice.PawnId = pawn.PawnId;
                changeJobNotice.CharacterJobData = activeCharacterJobData;
                changeJobNotice.EquipItemInfo = equipItemInfos;
                changeJobNotice.SetAcquirementParamList = skills;
                changeJobNotice.SetAbilityParamList = abilities;
                changeJobNotice.LearnNormalSkillParamList = normalSkills;
                changeJobNotice.EquipJobItemList = jobItems;
                // TODO: Unk0
                foreach(GameClient otherClient in server.ClientLookup.GetAll())
                {
                    otherClient.Send(changeJobNotice);
                }

                updateCharacterItemNtc.UpdateType = 0x29;
                client.Send(updateCharacterItemNtc);

                S2CJobChangePawnJobRes changeJobResponse = new S2CJobChangePawnJobRes();
                changeJobResponse.PawnId = pawn.PawnId;
                changeJobResponse.CharacterJobData = activeCharacterJobData;
                changeJobResponse.CharacterEquipList = characterEquipList;
                changeJobResponse.SetAcquirementParamList = skills;
                changeJobResponse.SetAbilityParamList = abilities;
                changeJobResponse.LearnNormalSkillParamList = normalSkills;
                changeJobResponse.EquipJobItemList = jobItems;
                changeJobResponse.Unk0.Unk0 = (byte) jobId;
                // changeJobResponse.Unk0.Unk1 = pawn.Storage.getAllStoragesAsCDataCharacterItemSlotInfoList(); // TODO: What
                // changeJobResponse.Unk1 // TODO: its the same thing as in CDataPawnInfo
                changeJobResponse.SpSkillList = pawn.SpSkillList;
                client.Send(changeJobResponse);
            }
            else
            {
                throw new Exception("Unknown character type");
            }
        }

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