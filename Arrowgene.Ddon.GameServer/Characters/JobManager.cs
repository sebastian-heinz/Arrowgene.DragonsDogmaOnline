#nullable enable
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.GameServer.Handler;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class JobManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobManager));
        private readonly DdonGameServer _Server;

        private static readonly List<EquipSlot> JEWELRY_SLOTS = new List<EquipSlot>
        {
            EquipSlot.Jewelry1, EquipSlot.Jewelry2, EquipSlot.Jewelry3, EquipSlot.Jewelry4, EquipSlot.Jewelry5,
        };

        public JobManager(DdonGameServer server)
        {
            _Server = server;
        }

        public void SetJob(GameClient client, CharacterCommon common, JobId jobId, DbConnection? connectionIn = null)
        {
            // TODO: Reject job change if there's no primary and secondary weapon for the new job in storage
            // (or give a lvl 1 weapon for free?)

            JobId oldJobId = common.Job;
            common.Job = jobId;

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            updateCharacterItemNtc.UpdateItemList.AddRange(SwapEquipmentAndStorage(client, common, oldJobId, jobId, EquipType.Performance, connectionIn));
            updateCharacterItemNtc.UpdateItemList.AddRange(SwapEquipmentAndStorage(client, common, oldJobId, jobId, EquipType.Visual, connectionIn));
            client.Send(updateCharacterItemNtc);

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
                _Server.Database.ReplaceCharacterJobData(common.CommonId, activeCharacterJobData);
            }

            // TODO: Figure out if CDataEquipItemInfo should be the equipment templates or just the currently equipped items
            List<CDataEquipItemInfo> equipItemInfos = common.Equipment.AsCDataEquipItemInfo(EquipType.Performance)
                .Union(common.Equipment.AsCDataEquipItemInfo(EquipType.Visual))
                .ToList();
            List<CDataCharacterEquipInfo> characterEquipList = common.Equipment.AsCDataCharacterEquipInfo(EquipType.Performance)
                .Union(common.Equipment.AsCDataCharacterEquipInfo(EquipType.Visual))
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
            List<CDataEquipJobItem> jobItems = common.EquipmentTemplate.JobItemsAsCDataEquipJobItem(common.Job);

            if (common is Character)
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

                foreach (GameClient otherClient in _Server.ClientLookup.GetAll())
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
                changeJobResponse.Unk0.Unk1 = character.Storage.GetAllStoragesAsCDataCharacterItemSlotInfoList();

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
                foreach (GameClient otherClient in _Server.ClientLookup.GetAll())
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

        private List<CDataItemUpdateResult> SwapEquipmentAndStorage(GameClient client, CharacterCommon common, JobId oldJobId, JobId newJobId, EquipType equipType, DbConnection? connectionIn = null)
        {
            List<CDataItemUpdateResult> itemUpdateResultList = new List<CDataItemUpdateResult>();
            List<Item?> oldEquipment = common.Equipment.GetItems(equipType);
            List<Item?> newEquipmentTemplate = common.EquipmentTemplate.GetEquipment(newJobId, equipType);
            for (int i = 0; i < newEquipmentTemplate.Count; i++)
            {
                byte equipSlot = (byte)(i+1);
                ushort equipmentStorageSlot = common.Equipment.GetStorageSlot(equipType, equipSlot);
                Item? oldEquipmentItem = oldEquipment[i];
                Item? newEquipmentTemplateItem = newEquipmentTemplate[i];
                if(oldEquipmentItem != null && newEquipmentTemplateItem == null)
                {
                    // Unequip item if the new job has no item equipped in this slot
                    // TODO: Attempt moving into other storages if the storage box is full
                    try
                    {
                        List<CDataItemUpdateResult> moveResult = _Server.ItemManager.MoveItem(_Server, client.Character, common.Equipment.Storage, equipmentStorageSlot, 1, client.Character.Storage.GetStorage(StorageType.StorageBoxNormal), 0, connectionIn);
                        itemUpdateResultList.AddRange(moveResult);
                    }
                    catch (ResponseErrorException ex)
                    {
                        if (ex.ErrorCode == ErrorCode.ERROR_CODE_CHARACTER_ITEM_NOT_FOUND)
                        {
                            // Handle the item not being in equipment storage
                            // This should probably return an error response instead? Logging and handling gracefully prevents messy situations, but may be undesirable
                            Logger.Error($"Failed to unequip item {oldEquipmentItem.UId} from {equipType} slot {equipSlot} of {oldJobId}. The item wasn't in the {common.Equipment.Storage.Type} slot {equipmentStorageSlot}");
                            common.EquipmentTemplate.SetEquipItem(null, oldJobId, equipType, equipSlot);
                            _Server.Database.DeleteEquipItem(common.CommonId, oldJobId, equipType, equipSlot, connectionIn);
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                }
                else if (newEquipmentTemplateItem != null && newEquipmentTemplateItem.UId != oldEquipmentItem?.UId)
                {
                    // Equip item to a slot, if said item is not already equipped. Search item in multiple storages
                    List<CDataItemUpdateResult>? moveResult = null;

                    //Search ahead for jewelry and remove in advance.
                    if (JEWELRY_SLOTS.Contains((EquipSlot)equipSlot))
                    {
                        foreach (EquipSlot jewelrySlot in JEWELRY_SLOTS)
                        {
                            if ((byte)jewelrySlot <= equipSlot) continue;
                            Item? oldJewelryItem = oldEquipment[(int)jewelrySlot - 1];
                            if (oldJewelryItem != null && newEquipmentTemplateItem.UId == oldJewelryItem?.UId)
                            {
                                ushort laterSlot = common.Equipment.GetStorageSlot(equipType, (byte)jewelrySlot);
                                try
                                {
                                    _Server.ItemManager.MoveItem(_Server, client.Character, common.Equipment.Storage, laterSlot, 1, client.Character.Storage.GetStorage(StorageType.StorageBoxNormal), 0);
                                }
                                catch (ResponseErrorException ex)
                                {
                                    if (ex.ErrorCode == ErrorCode.ERROR_CODE_CHARACTER_ITEM_NOT_FOUND)
                                    {
                                        // Do nothing, the slot is empty. Nothing to unequip
                                    }
                                    else
                                    {
                                        throw ex;
                                    }
                                }
                                break;
                            }
                        }
                    }

                    foreach (StorageType searchStorageType in ItemManager.BothStorageTypes)
                    {
                        try
                        {
                            moveResult = _Server.ItemManager.MoveItem(_Server, client.Character, client.Character.Storage.GetStorage(searchStorageType), newEquipmentTemplateItem.UId, 1, common.Equipment.Storage, equipmentStorageSlot, connectionIn);
                            itemUpdateResultList.AddRange(moveResult);
                            break;
                        }
                        catch (ResponseErrorException ex)
                        {
                            if (ex.ErrorCode == ErrorCode.ERROR_CODE_CHARACTER_ITEM_NOT_FOUND)
                            {
                                // Do nothing, item to equip not in this storage type.
                                // Even if it's not found in any of the StorageTypes of the loop, this isn't an error.
                                // It simply means that the player had an item equipped that was sold/discarded/etc
                                // since they switched jobs.
                            }
                            else
                            {
                                throw ex;
                            }
                        }
                    }

                    if (moveResult == null)
                    {
                        // Handle the item not being in storage anymore.
                        // Remove from template and unequip whatever was in that slot (if anything)
                        common.EquipmentTemplate.SetEquipItem(null, newJobId, equipType, equipSlot);
                        _Server.Database.DeleteEquipItem(common.CommonId, oldJobId, equipType, equipSlot, connectionIn);

                        try
                        {
                            // TODO: Attempt moving into other storages if the storage box is full
                            moveResult = _Server.ItemManager.MoveItem(_Server, client.Character, common.Equipment.Storage, equipmentStorageSlot, 1, client.Character.Storage.GetStorage(StorageType.StorageBoxNormal), 0, connectionIn);
                            itemUpdateResultList.AddRange(moveResult);
                        }
                        catch(ResponseErrorException ex)
                        {
                            if (ex.ErrorCode == ErrorCode.ERROR_CODE_CHARACTER_ITEM_NOT_FOUND)
                            {
                                // Do nothing, the slot is empty. Nothing to unequip
                            }
                            else
                            {
                                throw ex;
                            }
                        }
                    }
                }
            }

            // Persist job change in DB
            _Server.Database.UpdateCharacterCommonBaseInfo(common, connectionIn);

            return itemUpdateResultList;
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

        public void UnlockLearnedNormalSkill(AssetRepository assetRepo, IDatabase database, GameClient client, CharacterCommon character, JobId job, uint skillIndex)
        {
            CDataCharacterJobData characterJobData = character.CharacterJobDataList.Where(cjd => cjd.Job == job).Single();

            Dictionary<JobId, List<LearnedNormalSkill>> learnedNormalSkillsMap = assetRepo.LearnedNormalSkillsAsset.LearnedNormalSkills;

            if (!learnedNormalSkillsMap.ContainsKey(job) || skillIndex == 0 || ((skillIndex - 1) > learnedNormalSkillsMap[job].Count()))
            {
                // Something strange happened, either there is a new job (unlikely)
                // or there is a missing skill, or someone tried to craft a custom
                // packet to the server. Return back an error packet to the client.
                Logger.Error("Illegal request to unlock 'Learned Normal/Core Skill'");

                var S2CResult = new S2CSkillLearnNormalSkillRes()
                {
                    Error = 0xabaddeed
                };

                client.Send(S2CResult);
                return;
            }

            LearnedNormalSkill Skill = learnedNormalSkillsMap[job][(int)(skillIndex - 1)];
            if (characterJobData.JobPoint < Skill.JpCost || characterJobData.Lv < Skill.RequiredLevel)
            {
                // This shouldn't happen, but if it does, don't learn the skill and
                // return an error packet to the client.
                Logger.Error("Illegal request to unlock 'Learned Normal/Core Skill'");

                var S2CResult = new S2CSkillLearnNormalSkillRes()
                {
                    Error = 0xabaddeed
                };

                client.Send(S2CResult);
                return;
            }

            foreach (uint SkillNo in Skill.SkillNo)
            {
                List<CDataNormalSkillParam> matches = character.LearnedNormalSkills.Where(skill => skill != null && skill.Job == job && skill.SkillNo == SkillNo).ToList();
                if (matches.Count() == 0)
                {

                    CDataNormalSkillParam newSkill = new CDataNormalSkillParam()
                    {
                        Job = job,
                        Index = skillIndex, // 1, 2, 3 based offset from packet
                        SkillNo = SkillNo,  // Skill ID
                        PreSkillNo = 0
                    };

                    character.LearnedNormalSkills.Add(newSkill);
                    database.InsertIfNotExistsNormalSkillParam(character.CommonId, newSkill);
                }
            }

            // Subtract Job points and update the DB with the new result
            characterJobData.JobPoint -= Skill.JpCost;
            database.UpdateCharacterJobData(character.CommonId, characterJobData);

            if (character is Character)
            {
                client.Send(new S2CSkillLearnNormalSkillRes()
                {
                    Job = job,
                    SkillIndex = skillIndex,
                    NewJobPoint = characterJobData.JobPoint,
                });

                //Notify other members of the new skill.
                client.Party.SendToAll(new S2CSkillNormalSkillLearnNtc()
                {
                    CharacterId = ((Character)character).CharacterId,
                    NormalSkillData = new CDataContextNormalSkillData()
                    {
                        SkillIndex = (byte)skillIndex
                    }
                });
            }
            else
            {
                client.Send(new S2CSkillLearnPawnNormalSkillRes()
                {
                    PawnId = ((Pawn)character).PawnId,
                    Job = job,
                    SkillIndex = skillIndex,
                    NewJobPoint = characterJobData.JobPoint,
                });

                //Notify other members of the new skill.
                client.Party.SendToAll(new S2CSkillPawnNormalSkillLearnNtc()
                {
                    PawnId = ((Pawn)character).PawnId,
                    NormalSkillData = new CDataContextNormalSkillData()
                    {
                        SkillIndex = (byte)skillIndex
                    }
                });
            }
        }

        public void UnlockAbility(IDatabase database, GameClient client, CharacterCommon character, JobId job, uint abilityId, byte abilityLv)
        {
            //The job passed to this function can lie. 
            //By using the Augment search, you can buy augments from other jobs while passing a job parameter from the character's *current* job.
            //As such, always look up the ID to make sure you know what job it's really coming from.

            CDataAbilityParam abilityData = SkillGetAcquirableAbilityListHandler.GetAbilityFromId(abilityId);
            JobId owningJob = abilityData.Job;

            Logger.Debug($"Unlocking/upgrading ability {owningJob.ToString()}: {abilityId}");

            // Check if there is a learned ability of the same ID (This unlock is a level upgrade)
            Ability lowerLevelAbility = character.LearnedAbilities.Where(aug => aug != null && aug.AbilityId == abilityId).SingleOrDefault();

            if (lowerLevelAbility == null)
            {
                // New ability
                Ability newAbility = new Ability()
                {
                    Job = owningJob,
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

            uint jpCost = abilityData.Params.Where(x => x.Lv == abilityLv).Single().RequireJobPoint;

            CDataCharacterJobData learnedAbilityCharacterJobData = owningJob == 0
                ? character.ActiveCharacterJobData // Secret Augments -> Use current job's JP TODO: Verify if this is the correct behaviour
                : character.CharacterJobDataList.Where(jobData => jobData.Job == owningJob).Single(); // Job Augments -> Use that job's JP
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
                .Where(aug =>aug.AbilityId == abilityId)
                .SingleOrDefault();

            if (ability is null)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_SKILL_NOT_YET_LEARN);
            }

            character.EquippedAbilitiesDictionary[character.Job][slotNo - 1] = ability;

            database.ReplaceEquippedAbility(character.CommonId, character.Job, slotNo, ability);

            // Inform party members of the change
            AbilitySetNotifyParty(client, character, ability, slotNo);

            return ability;
        }

        public void RemoveAbility(IDatabase database, CharacterCommon character, byte slotNo)
        {
            List<Ability> equippedAbilities = character.EquippedAbilitiesDictionary[character.Job];

            equippedAbilities[slotNo - 1] = null; //Mark ability as null.
            equippedAbilities.RemoveAll(x => x is null); //Squash list.
            equippedAbilities.AddRange(Enumerable.Repeat<Ability>(null, 10 - equippedAbilities.Count)); //Resize back to 10.

            database.ReplaceEquippedAbilities(character.CommonId, character.Job, equippedAbilities);
        }

        public bool UnlockSecretAbility(CharacterCommon Character, SecretAbility secretAbilityType)
        {
            return _Server.Database.InsertSecretAbilityUnlock(Character.CommonId, secretAbilityType);
        }

        public static CDataPresetAbilityParam MakePresetAbilityParam(CharacterCommon character, List<Ability> abilities, byte presetNo, string presetName = "")
        {
            CDataPresetAbilityParam preset = new CDataPresetAbilityParam()
            {
                PresetNo = presetNo,
                PresetName = presetName,
            };

            byte i = 1;
            foreach (Ability ability in abilities)
            {
                if (ability is null) continue;
                preset.AbilityList.Add(ability.AsCDataSetAcquirementParam(i++));
            }

            return preset;
        }
    
        public void SetAbilityPreset(IDatabase database, GameClient client, CharacterCommon character, CDataPresetAbilityParam preset)
        {
            uint cost = 0;
            uint costMax = _Server.CharacterManager.GetMaxAugmentAllocation(character);
            foreach (var presetAbility in preset.AbilityList)
            {
                Ability ability = character.LearnedAbilities
                    .Where(aug => aug.AbilityId == presetAbility.AcquirementNo)
                    .SingleOrDefault();

                if (ability is null)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_SKILL_NOT_YET_LEARN);
                }

                cost += SkillGetAcquirableAbilityListHandler.GetAbilityFromId(presetAbility.AcquirementNo).Cost;

                if (cost > costMax)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_SKILL_COST_OVER);
                }
            }
            
            List<Ability> equippedAbilities = character.EquippedAbilitiesDictionary[character.Job];

            for (byte i = 0; i < 10; i++)
            {
                if (i >= preset.AbilityList.Count)
                {
                    equippedAbilities[i] = null;
                }
                else
                {
                    Ability ability = character.LearnedAbilities
                    .Where(aug => aug.AbilityId == preset.AbilityList[i].AcquirementNo)
                    .SingleOrDefault();

                    character.EquippedAbilitiesDictionary[character.Job][i] = ability;

                    AbilitySetNotifyParty(client, character, ability, (byte)(i + 1));
                }
            }

            //We wait until the end to do all of the DB updating in a single transaction.
            database.ReplaceEquippedAbilities(character.CommonId, character.Job, equippedAbilities);
        }

        private void AbilitySetNotifyParty(GameClient client, CharacterCommon character, Ability ability, byte slotNo)
        {
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
        }
    }
}
