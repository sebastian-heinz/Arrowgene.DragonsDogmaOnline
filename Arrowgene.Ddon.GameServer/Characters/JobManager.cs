using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer.Handler;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class JobManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobManager));
        private readonly IDatabase _Database;

        public JobManager(IDatabase database)
        {
            _Database = database;
        }

        public void SetJob(DdonServer<GameClient> server, GameClient client, CharacterCommon common, JobId jobId)
        {
            common.Job = jobId;

            server.Database.UpdateCharacterCommonBaseInfo(common);

            CDataCharacterJobData? activeCharacterJobData = common.ActiveCharacterJobData;

            if (activeCharacterJobData == null)
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

            List<CDataSetAcquirementParam> skills = common.EquippedCustomSkillsDictionary[jobId]
                .Select((x, idx) => x?.AsCDataSetAcquirementParam((byte)(idx + 1)))
                .Where(x => x != null)
                .ToList();
            List<CDataSetAcquirementParam> abilities = common.EquippedAbilitiesDictionary[jobId]
                .Select((x, idx) => x?.AsCDataSetAcquirementParam((byte)(idx + 1)))
                .Where(x => x != null)
                .ToList();
            List<CDataLearnNormalSkillParam> normalSkills = common.LearnedNormalSkills
                .Where(x => x.Job == common.Job)
                .Select(x => new CDataLearnNormalSkillParam(x))
                .ToList();
            List<CDataEquipJobItem> jobItems = common.Equipment.getJobItemsAsCDataEquipJobItem(common.Job);

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            // TODO: Move previous job equipment to storage box, and move new job equipment from storage box

            if (common is Character)
            {
                Character character = (Character)common;

                S2CJobChangeJobNtc changeJobNotice = new S2CJobChangeJobNtc();
                changeJobNotice.CharacterId = character.CharacterId;
                changeJobNotice.CharacterJobData = activeCharacterJobData;
                changeJobNotice.EquipItemInfo = equipItemInfos;
                changeJobNotice.SetAcquirementParamList = skills;
                changeJobNotice.SetAbilityParamList = abilities;
                changeJobNotice.LearnNormalSkillParamList = normalSkills;
                changeJobNotice.EquipJobItemList = jobItems;
                // TODO: Unk0

                foreach (GameClient otherClient in server.ClientLookup.GetAll())
                {
                    otherClient.Send(changeJobNotice);
                }

                updateCharacterItemNtc.UpdateType = ItemNoticeType.ChangeJob;
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
                changeJobResponse.Unk0.Unk0 = (byte)jobId;
                changeJobResponse.Unk0.Unk1 = character.Storage.getAllStoragesAsCDataCharacterItemSlotInfoList();

                client.Send(changeJobResponse);
            }
            else if (common is Pawn)
            {
                Pawn pawn = (Pawn)common;

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
                foreach (GameClient otherClient in server.ClientLookup.GetAll())
                {
                    otherClient.Send(changeJobNotice);
                }

                updateCharacterItemNtc.UpdateType = ItemNoticeType.ChangePawnJob;
                client.Send(updateCharacterItemNtc);

                S2CJobChangePawnJobRes changeJobResponse = new S2CJobChangePawnJobRes();
                changeJobResponse.PawnId = pawn.PawnId;
                changeJobResponse.CharacterJobData = activeCharacterJobData;
                changeJobResponse.CharacterEquipList = characterEquipList;
                changeJobResponse.SetAcquirementParamList = skills;
                changeJobResponse.SetAbilityParamList = abilities;
                changeJobResponse.LearnNormalSkillParamList = normalSkills;
                changeJobResponse.EquipJobItemList = jobItems;
                changeJobResponse.Unk0.Unk0 = (byte)jobId;
                // changeJobResponse.Unk0.Unk1 = pawn.Storage.getAllStoragesAsCDataCharacterItemSlotInfoList(); // TODO: What
                changeJobResponse.TrainingStatus = pawn.TrainingStatus.GetValueOrDefault(pawn.Job, new byte[64]);
                changeJobResponse.SpSkillList = pawn.SpSkills.GetValueOrDefault(pawn.Job, new List<CDataSpSkill>());
                client.Send(changeJobResponse);
            }
            else
            {
                throw new Exception("Unknown character type");
            }
        }

        public void UnlockSkill(IDatabase database, GameClient client, CharacterCommon character, JobId job, uint skillId, byte skillLv)
        {
            // Check if there is a learned skill of the same ID (This unlock is a level upgrade)
            CustomSkill lowerLevelSkill = character.LearnedCustomSkills.Where(skill => skill != null && skill.Job == job && skill.SkillId == skillId).SingleOrDefault();

            if (lowerLevelSkill == null)
            {
                // Add new skill
                CustomSkill newSkill = new CustomSkill()
                {
                    Job = job,
                    SkillId = skillId,
                    SkillLv = skillLv
                };
                character.LearnedCustomSkills.Add(newSkill);
                database.InsertLearnedCustomSkill(character.CommonId, newSkill);
            }
            else
            {
                // Upgrade existing skills
                lowerLevelSkill.SkillLv = skillLv;
                database.UpdateLearnedCustomSkill(character.CommonId, lowerLevelSkill);
            }

            // EX Skills
            if (skillLv == 9)
            {
                // EX T Skill
                uint exSkillTId = skillId + 100;
                CDataSkillParam? exSkillT = SkillGetAcquirableSkillListHandler.AllSkills.Where(skill => skill.Job == job && skill.SkillNo == exSkillTId).SingleOrDefault();
                if (exSkillT != null)
                {
                    // Add new skill
                    CustomSkill newExSkillT = new CustomSkill()
                    {
                        Job = job,
                        SkillId = exSkillTId,
                        SkillLv = 1
                    };
                    character.LearnedCustomSkills.Add(newExSkillT);
                    database.InsertLearnedCustomSkill(character.CommonId, newExSkillT);
                }
            }
            else if (skillLv == 10)
            {
                // EX P Skill
                uint exSkillPId = skillId + 200;
                CDataSkillParam? exSkillP = SkillGetAcquirableSkillListHandler.AllSkills.Where(skill => skill.Job == job && skill.SkillNo == exSkillPId).SingleOrDefault();
                if (exSkillP != null)
                {
                    // Add new skill
                    CustomSkill newExSkillP = new CustomSkill()
                    {
                        Job = job,
                        SkillId = exSkillPId,
                        SkillLv = 1
                    };
                    character.LearnedCustomSkills.Add(newExSkillP);
                    database.InsertLearnedCustomSkill(character.CommonId, newExSkillP);
                }
            }

            uint jpCost = SkillGetAcquirableSkillListHandler.AllSkills
                .Where(skill => skill.Job == job && skill.SkillNo == skillId)
                .SelectMany(skill => skill.Params)
                .Where(skillParams => skillParams.Lv == skillLv)
                .Select(skillParams => skillParams.RequireJobPoint)
                .Single();

            // TODO: Check that this doesn't end up negative
            CDataCharacterJobData learnedSkillCharacterJobData = character.CharacterJobDataList.Where(jobData => jobData.Job == job).Single();
            learnedSkillCharacterJobData.JobPoint -= jpCost;
            database.UpdateCharacterJobData(character.CommonId, learnedSkillCharacterJobData);

            if (character is Character)
            {
                client.Send(new S2CSkillLearnSkillRes()
                {
                    Job = job,
                    NewJobPoint = learnedSkillCharacterJobData.JobPoint,
                    SkillId = skillId,
                    SkillLv = skillLv
                });

                //Find on the palletes (if any) where the skill is set and notify party. Can occur at two locations (Main + Secondary) for players.
                List<CustomSkill?> equippedCustomSkillList = character.EquippedCustomSkillsDictionary[job];
                var slotIndices = Enumerable.Range(0, equippedCustomSkillList.Count)
                    .Where(i => equippedCustomSkillList[i] != null && equippedCustomSkillList[i].SkillId == skillId)
                    .ToList();
                foreach (int slotIndex in slotIndices)
                {
                    client.Party.SendToAll(new S2CSkillCustomSkillSetNtc()
                    {
                        CharacterId = ((Character)character).CharacterId,
                        ContextAcquirementData = new CDataContextAcquirementData()
                        {
                            SlotNo = (byte)(slotIndex + 1),
                            AcquirementNo = skillId,
                            AcquirementLv = skillLv
                        }
                    });
                }       
            }
            else if (character is Pawn)
            {
                client.Send(new S2CSkillLearnPawnSkillRes()
                {
                    PawnId = ((Pawn)character).PawnId,
                    Job = job,
                    NewJobPoint = learnedSkillCharacterJobData.JobPoint,
                    SkillId = skillId,
                    SkillLv = skillLv
                });

                //Find on the palletes (if any) where the skill is set. 
                List<CustomSkill?> equippedCustomSkillList = character.EquippedCustomSkillsDictionary[job];
                var slotIndices = Enumerable.Range(0, equippedCustomSkillList.Count)
                    .Where(i => equippedCustomSkillList[i] != null && equippedCustomSkillList[i].SkillId == skillId)
                    .ToList();
                foreach (int slotIndex in slotIndices)
                {
                    client.Party.SendToAll(new S2CSkillPawnCustomSkillSetNtc()
                    {
                        PawnId = ((Pawn)character).PawnId,
                        ContextAcquirementData = new CDataContextAcquirementData()
                        {
                            SlotNo = (byte)(slotIndex + 1),
                            AcquirementNo = skillId,
                            AcquirementLv = skillLv
                        }
                    });
                }
            }
        }

        public CustomSkill SetSkill(IDatabase database, GameClient client, CharacterCommon character, JobId job, byte slotNo, uint skillId, byte skillLv)
        {
            // Remove skill from other slots in the same palette
            int paletteMask = slotNo & 0x10;
            for (int i = 0; i < character.EquippedCustomSkillsDictionary[job].Count; i++)
            {
                byte removedSkillSlotNo = (byte)(i + 1);
                CustomSkill removedSkill = character.EquippedCustomSkillsDictionary[job][i];
                if (removedSkill != null && removedSkill.Job == job && removedSkill.SkillId == skillId && (removedSkillSlotNo & 0x10) == paletteMask)
                {
                    character.EquippedCustomSkillsDictionary[job][i] = null;
                    database.DeleteEquippedCustomSkill(character.CommonId, job, removedSkillSlotNo);
                }
            }

            // Add skill to the requested slot
            CustomSkill skill = character.LearnedCustomSkills.Where(skill => skill.Job == job && skill.SkillId == skillId).Single();
            character.EquippedCustomSkillsDictionary[job][slotNo - 1] = skill;
            database.ReplaceEquippedCustomSkill(character.CommonId, slotNo, skill);

            // Inform party members of the change
            if (job == character.Job)
            {
                if (character is Character)
                {
                    client.Party.SendToAll(new S2CSkillCustomSkillSetNtc()
                    {
                        CharacterId = ((Character)character).CharacterId,
                        ContextAcquirementData = skill.AsCDataContextAcquirementData(slotNo)
                    });
                }
                else if (character is Pawn)
                {
                    client.Party.SendToAll(new S2CSkillPawnCustomSkillSetNtc()
                    {
                        PawnId = ((Pawn)character).PawnId,
                        ContextAcquirementData = skill.AsCDataContextAcquirementData(slotNo)
                    });
                }
            }

            return skill;
        }

        public IEnumerable<byte> ChangeExSkill(IDatabase database, GameClient client, CharacterCommon character, JobId job, uint skillId)
        {
            CustomSkill exSkill = character.LearnedCustomSkills
                .Where(skill => skill.Job == job && skill.SkillId == skillId)
                .Single();

            CustomSkill affectedSkill = character.LearnedCustomSkills
                .Where(skill => skill.Job == job && skill.SkillId == GetBaseSkillId(skillId))
                .Single();

            List<byte> affectedSlots = new List<byte>();
            for (int i = 0; i < character.EquippedCustomSkillsDictionary[job].Count; i++)
            {
                CustomSkill? equippedSkill = character.EquippedCustomSkillsDictionary[job][i];
                byte slotNo = (byte)(i + 1);
                if (equippedSkill != null && GetBaseSkillId(equippedSkill.SkillId) == GetBaseSkillId(affectedSkill.SkillId))
                {
                    SetSkill(database, client, character, exSkill.Job, slotNo, exSkill.SkillId, exSkill.SkillLv);
                    affectedSlots.Add(slotNo);
                }
            }
            return affectedSlots;
        }

        private uint GetBaseSkillId(uint skillId)
        {
            return skillId % 100;
        }

        public void RemoveSkill(IDatabase database, CharacterCommon character, JobId job, byte slotNo)
        {
            character.EquippedCustomSkillsDictionary[job][slotNo - 1] = null;

            // TODO: Error handling
            database.DeleteEquippedCustomSkill(character.CommonId, job, slotNo);

            // I haven't found a packet to notify this to other players
            // From what I tested it doesn't seem to be necessary
        }

        public void UnlockLearnedNormalSkill(AssetRepository AssetRepo, IDatabase Database, GameClient Client, CharacterCommon Character, JobId Job, uint SkillIndex)
        {
            CDataCharacterJobData CharacterJobData = Character.CharacterJobDataList.Where(cjd => cjd.Job == Job).Single();

            Dictionary<JobId, List<LearnedNormalSkill>> LearnedNormalSkillsMap = AssetRepo.LearnedNormalSkillsAsset.LearnedNormalSkills;

            if (!LearnedNormalSkillsMap.ContainsKey(Job) || SkillIndex == 0 || ((SkillIndex - 1) > LearnedNormalSkillsMap[Job].Count()))
            {
                // Something strange happened, either there is a new job (unlikely)
                // or there is a missing skill, or someone tried to craft a custom
                // packet to the server. Return back an error packet to the client.
                Logger.Error("Illegal request to unlock 'Learned Normal/Core Skill'");

                var S2CResult = new S2CSkillLearnNormalSkillRes()
                {
                    Error = 0xabaddeed
                };

                Client.Send(S2CResult);
                return;
            }

            LearnedNormalSkill Skill = LearnedNormalSkillsMap[Job][(int)(SkillIndex - 1)];
            if (CharacterJobData.JobPoint < Skill.JpCost || CharacterJobData.Lv < Skill.RequiredLevel)
            {
                // This shouldn't happen, but if it does, don't learn the skill and
                // return an error packet to the client.
                Logger.Error("Illegal request to unlock 'Learned Normal/Core Skill'");

                var S2CResult = new S2CSkillLearnNormalSkillRes()
                {
                    Error = 0xabaddeed
                };

                Client.Send(S2CResult);
                return;
            }

            foreach (uint SkillNo in Skill.SkillNo)
            {
                List<CDataNormalSkillParam> Matches = Character.LearnedNormalSkills.Where(skill => skill != null && skill.Job == Job && skill.SkillNo == SkillNo).ToList();
                if (Matches.Count() == 0)
                {

                    CDataNormalSkillParam NewSkill = new CDataNormalSkillParam()
                    {
                        Job = Job,
                        Index = SkillIndex, // 1, 2, 3 based offset from packet
                        SkillNo = SkillNo,  // Skill ID
                        PreSkillNo = 0
                    };

                    Character.LearnedNormalSkills.Add(NewSkill);
                    Database.InsertIfNotExistsNormalSkillParam(Character.CommonId, NewSkill);
                }
            }

            // Subtract Job points and update the DB with the new result
            CharacterJobData.JobPoint -= Skill.JpCost;
            Database.UpdateCharacterJobData(Character.CommonId, CharacterJobData);

            if (Character is Character)
            {
                var Result = new S2CSkillLearnNormalSkillRes()
                {
                    Job = Job,
                    SkillIndex = SkillIndex,
                    NewJobPoint = CharacterJobData.JobPoint,
                };

                Client.Send(Result);
            }
            else
            {
                var Result = new S2CSkillLearnPawnNormalSkillRes()
                {
                    PawnId = ((Pawn)Character).PawnId,
                    Job = Job,
                    SkillIndex = SkillIndex,
                    NewJobPoint = CharacterJobData.JobPoint,
                };

                Client.Send(Result);
            }

            // TODO: Send data to rest of party
            // TODO: S2C_NORMAL_SKILL_LEARN_NTC currently not defined
            // TODO: Need to investigate ID and layout
            // Client.Party.SendToAll(S2C_NORMAL_SKILL_LEARN_NTC)
        }

        public void UnlockAbility(IDatabase database, GameClient client, CharacterCommon character, JobId job, uint abilityId, byte abilityLv)
        {
            // Check if there is a learned ability of the same ID (This unlock is a level upgrade)
            Ability lowerLevelAbility = character.LearnedAbilities.Where(aug => aug != null && aug.Job == job && aug.AbilityId == abilityId).SingleOrDefault();

            Logger.Debug($"The Ability ID is {abilityId}");

            if (lowerLevelAbility == null)
            {
                // New ability
                Ability newAbility = new Ability()
                {
                    Job = job,
                    AbilityId = abilityId,
                    AbilityLv = abilityLv
                };
                character.LearnedAbilities.Add(newAbility);
                database.InsertLearnedAbility(character.CommonId, newAbility);
            }
            else
            {
                // Level upgrade
                lowerLevelAbility.AbilityLv = abilityLv;
                database.UpdateLearnedAbility(character.CommonId, lowerLevelAbility);
            }

            List<CDataAbilityParam> Abilities = (job == 0) ? SkillGetAcquirableAbilityListHandler.AllSecretAbilities : SkillGetAcquirableAbilityListHandler.AllAbilities;
            uint jpCost = Abilities
                .Where(aug => aug.Job == job && aug.AbilityNo == abilityId)
                .SelectMany(aug => aug.Params)
                .Where(augParams => augParams.Lv == abilityLv)
                .Select(augParams => augParams.RequireJobPoint)
                .Single();

            // TODO: Check that this doesn't end up negative
            CDataCharacterJobData learnedAbilityCharacterJobData = job == 0
                ? character.ActiveCharacterJobData // Secret Augments -> Use current job's JP TODO: Verify if this is the correct behaviour
                : character.CharacterJobDataList.Where(jobData => jobData.Job == job).Single(); // Job Augments -> Use that job's JP
            learnedAbilityCharacterJobData.JobPoint -= jpCost;
            database.UpdateCharacterJobData(character.CommonId, learnedAbilityCharacterJobData);

            if (character is Character)
            {
                client.Send(new S2CSkillLearnAbilityRes()
                {
                    Job = learnedAbilityCharacterJobData.Job,
                    NewJobPoint = learnedAbilityCharacterJobData.JobPoint,
                    AbilityId = abilityId,
                    AbilityLv = abilityLv
                });
            }
            else if (character is Pawn)
            {
                client.Send(new S2CSkillLearnPawnAbilityRes()
                {
                    PawnId = ((Pawn)character).PawnId,
                    Job = learnedAbilityCharacterJobData.Job,
                    NewJobPoint = learnedAbilityCharacterJobData.JobPoint,
                    AbilityId = abilityId,
                    AbilityLv = abilityLv
                });
            }
        }

        public Ability SetAbility(IDatabase database, GameClient client, CharacterCommon character, JobId abilityJob, byte slotNo, uint abilityId, byte abilityLv)
        {
            Ability ability = character.LearnedAbilities
                .Where(aug => aug.Job == abilityJob && aug.AbilityId == abilityId && aug.AbilityLv == abilityLv)
                .Single();

            character.EquippedAbilitiesDictionary[character.Job][slotNo - 1] = ability;

            database.ReplaceEquippedAbility(character.CommonId, character.Job, slotNo, ability);

            // Inform party members of the change
            if (character is Character)
            {
                client.Party.SendToAll(new S2CSkillAbilitySetNtc()
                {
                    CharacterId = ((Character)character).CharacterId,
                    ContextAcquirementData = ability.AsCDataContextAcquirementData(slotNo)
                });
            }
            else if (character is Pawn)
            {
                client.Party.SendToAll(new S2CSkillPawnAbilitySetNtc()
                {
                    PawnId = ((Pawn)character).PawnId,
                    ContextAcquirementData = ability.AsCDataContextAcquirementData(slotNo)
                });
            }

            return ability;
        }

        public void RemoveAbility(IDatabase database, CharacterCommon character, byte slotNo)
        {
            // TODO: Performance
            List<Ability> equippedAbilities = character.EquippedAbilitiesDictionary[character.Job];
            lock (equippedAbilities)
            {
                byte removedAbilitySlotNo = Byte.MaxValue;
                for (int i = 0; i < equippedAbilities.Count; i++)
                {
                    Ability equippedAbility = equippedAbilities[i];
                    byte equippedAbilitySlotNo = (byte)(i + 1);
                    if (character.Job == character.Job && equippedAbilitySlotNo == slotNo)
                    {
                        equippedAbilities[i] = null;
                        removedAbilitySlotNo = equippedAbilitySlotNo;
                        break;
                    }
                }

                for (int i = 0; i < equippedAbilities.Count; i++)
                {
                    Ability equippedAbility = equippedAbilities[i];
                    byte equippedAbilitySlotNo = (byte)(i + 1);
                    if (character.Job == character.Job)
                    {
                        if (equippedAbilitySlotNo > removedAbilitySlotNo)
                        {
                            equippedAbilitySlotNo--;
                        }
                    }
                }
            }

            database.ReplaceEquippedAbilities(character.CommonId, character.Job, equippedAbilities);

            // Same as skills, i haven't found an Ability off NTC. It may not be required
        }

        public bool UnlockSecretAbility(CharacterCommon Character, SecretAbility secretAbilityType)
        {
            return _Database.InsertSecretAbilityUnlock(Character.CommonId, secretAbilityType);
        }
    }
}
