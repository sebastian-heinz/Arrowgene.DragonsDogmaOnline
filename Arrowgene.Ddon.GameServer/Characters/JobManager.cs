using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Infrastructure;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.GameServer.Handler;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public static class LearnedNormalSkills
    {
        internal static class Alchemist
        {
            public const uint AlchemicalRadius = 2;
            public const uint ClusterMagis = 4;
            public const uint RexLeap = 10;
            public const uint ConclusterMagis = 12;
        }

        internal static class  ElementArcher
        {
            public const uint Seeker = 4;
            public const uint TrueAid = 8;
            public const uint GodsEye = 11;
        }

        internal static class Fighter
        {
            public const uint DireOnslaught = 2;
            public const uint GuardBreak = 4;
            public const uint ControlledFall = 12;
        }

        internal static class HighScepter
        {
            public const uint ArcSlash = 2;
            public const uint ControlledFall = 13;
        }

        internal static class Hunter
        {
            public const uint QuickBowStyle = 3;
            public const uint QuickChargeStyle = 6;
            public const uint KeenSight = 7;
        }

        internal static class Priest
        {
            public const uint HealAura = 3;
            public const uint HolyAura = 5;
            public const uint Floating = 8;
        }

        internal static class Seeker
        {
            public const uint RoundhouseKick = 2;
            public const uint HundredKisses = 4;
            public const uint DoubleJump = 14;
            public const uint HundredSlashes = 16;
        }

        internal static class ShieldSage
        {
            public const uint ElementChange = 4;
            public const uint Attract = 10;
            public const uint ShieldSequence = 12;
        }

        internal static class Sorcerer
        {
            public const uint MagickBoost = 3;
            public const uint BackMove = 5;
            public const uint Floating = 10;
        }

        internal static class SpiritLancer
        {
            public const uint ThrustMore = 2;
            public const uint Aid = 4;
            public const uint FallMove = 12;
        }

        internal static class Warrior
        {
            public const uint Devastate = 2;
            public const uint SavageLash = 4;
            public const uint ControlledFall = 9;
        }
    }

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

            List<CDataSetAcquirementParam> skills = common.EquippedCustomSkillsDictionary[jobId]
                .Select((x, idx) => x?.AsCDataSetAcquirementParam((byte)(idx+1)))
                .Where(x => x != null)
                .ToList();
            List<CDataSetAcquirementParam> abilities = common.EquippedAbilitiesDictionary[jobId]
                .Select((x, idx) => x?.AsCDataSetAcquirementParam((byte)(idx+1)))
                .Where(x => x != null)
                .ToList();
            List<CDataLearnNormalSkillParam> normalSkills = common.LearnedNormalSkills
                .Where(x => x.Job == common.Job)
                .Select(x => new CDataLearnNormalSkillParam(x))
                .ToList();
            List<CDataEquipJobItem> jobItems = common.Equipment.getJobItemsAsCDataEquipJobItem(common.Job);

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

        public void UnlockSkill(IDatabase database, GameClient client, CharacterCommon character, JobId job, uint skillId, byte skillLv)
        {
            // Check if there is a learned skill of the same ID (This unlock is a level upgrade)
            CustomSkill lowerLevelSkill = character.LearnedCustomSkills.Where(skill => skill != null && skill.Job == job && skill.SkillId == skillId).SingleOrDefault();

            if(lowerLevelSkill == null)
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
            if(skillLv == 9)
            {
                // EX T Skill
                uint exSkillTId = skillId+100;
                CDataSkillParam? exSkillT = SkillGetAcquirableSkillListHandler.AllSkills.Where(skill => skill.Job == job && skill.SkillNo == exSkillTId).SingleOrDefault();
                if(exSkillT != null)
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
            else if(skillLv == 10)
            {
                // EX P Skill
                uint exSkillPId = skillId+200;
                CDataSkillParam? exSkillP = SkillGetAcquirableSkillListHandler.AllSkills.Where(skill => skill.Job == job && skill.SkillNo == exSkillPId).SingleOrDefault();
                if(exSkillP != null)
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

            if(character is Character)
            {
                client.Send(new S2CSkillLearnSkillRes()
                {
                    Job = job,
                    NewJobPoint = learnedSkillCharacterJobData.JobPoint,
                    SkillId = skillId,
                    SkillLv = skillLv
                });
            }
            else if(character is Pawn)
            {
                client.Send(new S2CSkillLearnPawnSkillRes()
                {
                    PawnId = ((Pawn) character).PawnId,
                    Job = job,
                    NewJobPoint = learnedSkillCharacterJobData.JobPoint,
                    SkillId = skillId,
                    SkillLv = skillLv
                });
            }
        }

        public CustomSkill SetSkill(IDatabase database, GameClient client, CharacterCommon character, JobId job, byte slotNo, uint skillId, byte skillLv)
        {
            // Remove skill from other slots in the same palette
            int paletteMask = slotNo & 0x10;
            for (int i = 0; i < character.EquippedCustomSkillsDictionary[job].Count; i++)
            {
                byte removedSkillSlotNo = (byte)(i+1);
                CustomSkill removedSkill = character.EquippedCustomSkillsDictionary[job][i];
                if (removedSkill != null && removedSkill.Job == job && removedSkill.SkillId == skillId && (removedSkillSlotNo&0x10) == paletteMask)
                {
                    character.EquippedCustomSkillsDictionary[job][i] = null;
                    database.DeleteEquippedCustomSkill(character.CommonId, job, removedSkillSlotNo);
                }
            }

            // Add skill to the requested slot
            CustomSkill skill = character.LearnedCustomSkills.Where(skill => skill.Job == job && skill.SkillId == skillId).Single();
            character.EquippedCustomSkillsDictionary[job][slotNo-1] = skill;
            database.ReplaceEquippedCustomSkill(character.CommonId, slotNo, skill);

            // Inform party members of the change
            if(job == character.Job)
            {
                if(character is Character)
                {
                    client.Party.SendToAll(new S2CSkillCustomSkillSetNtc()
                    {
                        CharacterId = ((Character) character).CharacterId,
                        ContextAcquirementData = skill.AsCDataContextAcquirementData(slotNo)
                    });
                }
                else if(character is Pawn)
                {
                    client.Party.SendToAll(new S2CSkillPawnCustomSkillSetNtc()
                    {
                        PawnId = ((Pawn) character).PawnId,
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
            for(int i=0; i<character.EquippedCustomSkillsDictionary[job].Count; i++)
            {
                CustomSkill? equippedSkill = character.EquippedCustomSkillsDictionary[job][i];
                byte slotNo = (byte)(i+1);
                if(equippedSkill != null && GetBaseSkillId(equippedSkill.SkillId) == GetBaseSkillId(affectedSkill.SkillId))
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
            character.EquippedCustomSkillsDictionary[job][slotNo-1] = null;

            // TODO: Error handling
            database.DeleteEquippedCustomSkill(character.CommonId, job, slotNo);

            // I haven't found a packet to notify this to other players
            // From what I tested it doesn't seem to be necessary
        }

        static readonly Dictionary<JobId, List<uint[]>> gLearnedNormalSkillMap = new ()
        {
            [JobId.Alchemist] = new List<uint[]>() {
                    new uint[] { LearnedNormalSkills.Alchemist.AlchemicalRadius },
                    new uint[] { LearnedNormalSkills.Alchemist.ClusterMagis, LearnedNormalSkills.Alchemist.ConclusterMagis },
                    new uint[] { LearnedNormalSkills.Alchemist.RexLeap }
            },
            [JobId.ElementArcher] = new List<uint[]>() {
                    new uint[] { LearnedNormalSkills.ElementArcher.Seeker },
                    new uint[] { LearnedNormalSkills.ElementArcher.TrueAid },
                    new uint[] { LearnedNormalSkills.ElementArcher.GodsEye }
            },
            [JobId.Fighter] = new List<uint[]>() {
                    new uint[] { LearnedNormalSkills.Fighter.DireOnslaught },
                    new uint[] { LearnedNormalSkills.Fighter.GuardBreak },
                    new uint[] { LearnedNormalSkills.Fighter.ControlledFall }
            },
            [JobId.HighScepter] = new List<uint[]>() {
                    new uint[] { LearnedNormalSkills.HighScepter.ArcSlash },
                    new uint[] { LearnedNormalSkills.HighScepter.ControlledFall },
            },
            [JobId.Hunter] = new List<uint[]>() {
                    new uint[] { LearnedNormalSkills.Hunter.QuickBowStyle },
                    new uint[] { LearnedNormalSkills.Hunter.QuickChargeStyle },
                    new uint[] { LearnedNormalSkills.Hunter.KeenSight }
            },
            [JobId.Priest] = new List<uint[]>() {
                    new uint[] { LearnedNormalSkills.Priest.HealAura },
                    new uint[] { LearnedNormalSkills.Priest.HolyAura },
                    new uint[] { LearnedNormalSkills.Priest.Floating }
            },
            [JobId.ShieldSage] = new List<uint[]>() {
                    new uint[] { LearnedNormalSkills.ShieldSage.ElementChange },
                    new uint[] { LearnedNormalSkills.ShieldSage.ShieldSequence },
                    new uint[] { LearnedNormalSkills.ShieldSage.Attract }
            },
            [JobId.Seeker] = new List<uint[]>() {
                    new uint[] { LearnedNormalSkills.Seeker.RoundhouseKick },
                    new uint[] { LearnedNormalSkills.Seeker.HundredKisses, LearnedNormalSkills.Seeker.HundredSlashes },
                    new uint[] { LearnedNormalSkills.Seeker.DoubleJump }
            },
            [JobId.Sorcerer] = new List<uint[]>() {
                    new uint[] { LearnedNormalSkills.Sorcerer.MagickBoost },
                    new uint[] { LearnedNormalSkills.Sorcerer.BackMove},
                    new uint[] { LearnedNormalSkills.Sorcerer.Floating }
            },
            [JobId.SpiritLancer] = new List<uint[]>() {
                    new uint[] { LearnedNormalSkills.SpiritLancer.ThrustMore },
                    new uint[] { LearnedNormalSkills.SpiritLancer.Aid },
                    new uint[] { LearnedNormalSkills.SpiritLancer.FallMove }
            },
            [JobId.Warrior] = new List<uint[]>() {
                    new uint[] { LearnedNormalSkills.Warrior.Devastate },
                    new uint[] { LearnedNormalSkills.Warrior.SavageLash },
                    new uint[] { LearnedNormalSkills.Warrior.ControlledFall }
            },
        };

        public void UnlockLearnedNormalSkill(IDatabase database, GameClient client, CharacterCommon character, JobId Job, uint SkillIndex)
        {
            CDataCharacterJobData characterJobData = character.CharacterJobDataList.Where(cjd => cjd.Job == Job).Single();

            if (!gLearnedNormalSkillMap.ContainsKey(Job) || SkillIndex == 0 || ((SkillIndex - 1) > gLearnedNormalSkillMap[Job].Count()))
            {
                // Something strange happened, either there is a new job (unlikely)
                // or there is a missing skill, or someone tried to craft a custom
                // packet to the server. For now, just return back the same
                // data requested so the client doesn't hang.
                var S2CSkillLearnNormalSkillRes = new S2CSkillLearnNormalSkillRes()
                {
                    Job = Job,
                    SkillIndex = 0,
                    NewJobPoint = characterJobData.JobPoint,
                };

                client.Send(S2CSkillLearnNormalSkillRes);
                return;
            }

            List<uint[]> SkillMapping = gLearnedNormalSkillMap[Job];
            foreach (uint SkillNo in SkillMapping[(int)(SkillIndex - 1)])
            {
                List<CDataNormalSkillParam> Matches = character.LearnedNormalSkills.Where(skill => skill != null && skill.Job == Job && skill.SkillNo == SkillNo).ToList();
                if (Matches.Count() == 0)
                {
                    CDataNormalSkillParam NewSkill = new CDataNormalSkillParam()
                    {
                        Job = Job,
                        Index = SkillIndex, // 1, 2, 3 based offset from packet
                        SkillNo = SkillNo,  // Related to offsets in gNormalSkillMap
                    };

                    character.LearnedNormalSkills.Add(NewSkill);
                    database.InsertIfNotExistsNormalSkillParam(character.CommonId, NewSkill);
                }
            }

            // TODO: Subtract Job Points

            var Result = new S2CSkillLearnNormalSkillRes()
            {
                Job = Job,
                SkillIndex = SkillIndex,
                NewJobPoint = characterJobData.JobPoint,
            };

            client.Send(Result);
        }

        public void UnlockAbility(IDatabase database, GameClient client, CharacterCommon character, JobId job, uint abilityId, byte abilityLv)
        {
            // Check if there is a learned ability of the same ID (This unlock is a level upgrade)
            Ability lowerLevelAbility = character.LearnedAbilities.Where(aug => aug != null && aug.Job == job && aug.AbilityId == abilityId).SingleOrDefault();

            if(lowerLevelAbility == null)
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

            uint jpCost = SkillGetAcquirableAbilityListHandler.AllAbilities
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

            if(character is Character)
            {
                client.Send(new S2CSkillLearnAbilityRes()
                {
                    Job = learnedAbilityCharacterJobData.Job,
                    NewJobPoint = learnedAbilityCharacterJobData.JobPoint,
                    AbilityId = abilityId,
                    AbilityLv = abilityLv
                });
            }
            else if(character is Pawn)
            {
                client.Send(new S2CSkillLearnPawnAbilityRes()
                {
                    PawnId = ((Pawn) character).PawnId,
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

            character.EquippedAbilitiesDictionary[character.Job][slotNo-1] = ability;

            database.ReplaceEquippedAbility(character.CommonId, character.Job, slotNo, ability);

            // Inform party members of the change
            if(character is Character)
            {
                client.Party.SendToAll(new S2CSkillAbilitySetNtc()
                {
                    CharacterId = ((Character) character).CharacterId,
                    ContextAcquirementData = ability.AsCDataContextAcquirementData(slotNo)
                });
            }
            else if(character is Pawn)
            {
                client.Party.SendToAll(new S2CSkillPawnAbilitySetNtc()
                {
                    PawnId = ((Pawn) character).PawnId,
                    ContextAcquirementData = ability.AsCDataContextAcquirementData(slotNo)
                });
            }

            return ability;
        }

        public void RemoveAbility(IDatabase database, CharacterCommon character, byte slotNo)
        {
            // TODO: Performance
            List<Ability> equippedAbilities = character.EquippedAbilitiesDictionary[character.Job];
            lock(equippedAbilities)
            {
                byte removedAbilitySlotNo = Byte.MaxValue;
                for(int i=0; i<equippedAbilities.Count; i++)
                {
                    Ability equippedAbility = equippedAbilities[i];
                    byte equippedAbilitySlotNo = (byte)(i+1);
                    if(character.Job == character.Job && equippedAbilitySlotNo == slotNo)
                    {
                        equippedAbilities[i] = null;
                        removedAbilitySlotNo = equippedAbilitySlotNo;
                        break;
                    }
                }

                for(int i=0; i<equippedAbilities.Count; i++)
                {
                    Ability equippedAbility = equippedAbilities[i];
                    byte equippedAbilitySlotNo = (byte)(i+1);
                    if(character.Job == character.Job)
                    {
                        if(equippedAbilitySlotNo > removedAbilitySlotNo)
                        {
                            equippedAbilitySlotNo--;
                        }
                    }
                }
            }

            database.ReplaceEquippedAbilities(character.CommonId, character.Job, equippedAbilities);

            // Same as skills, i haven't found an Ability off NTC. It may not be required
        }
    }
}
