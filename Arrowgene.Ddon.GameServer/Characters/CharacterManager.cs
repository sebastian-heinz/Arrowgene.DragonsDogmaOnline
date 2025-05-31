using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class CharacterManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterManager));

        public static readonly uint BASE_HEALTH = 760U;
        public static readonly uint BASE_STAMINA = 450U;
        public static readonly uint BBM_BASE_HEALTH = 990U;
        public static readonly uint BBM_BASE_STAMINA = 589U;

        public static readonly uint DEFAULT_RING_COUNT = 1;
        public static readonly uint BASE_ABILITY_COST_AMOUNT = 15;

        public static readonly uint MAX_PLAYER_HP = uint.MaxValue;
        public static readonly uint MAX_PLAYER_STAMINA = uint.MaxValue;

        private readonly DdonGameServer Server;

        public CharacterManager(DdonGameServer server)
        {
            Server = server;
        }

        public Character SelectCharacter(GameClient client, uint characterId, DbConnection? connectionIn = null)
        {
            Character character = SelectCharacter(characterId, connectionIn:connectionIn);
            client.Character = character;
            client.UpdateIdentity();

            return character;
        }

        public Character SelectCharacter(uint characterId, bool fetchPawns = true, DbConnection? connectionIn = null)
        {
            return Server.Database.ExecuteQuerySafe(connectionIn, connectionIn =>
            {
                Character character = Server.Database.SelectCharacter(characterId, connectionIn);
                if (character == null)
                {
                    return null;
                }

                character.Server = Server.AssetRepository.ServerList.Where(server => server.Id == Server.Id).Single().ToCDataGameServerListInfo();
                
                // Apply Emblem stats before setting up character equipment
                character.JobEmblems = Server.JobEmblemManager.InitializeEmblemData(character, connectionIn);
                character.Equipment = character.Storage.GetCharacterEquipment();

                character.EmblemStatList = Server.JobEmblemManager.GetEmblemStatsForCurrentJob(character);

                character.ContentsReleased = GetContentsReleased(character, connectionIn);
                character.WorldManageUnlocks = GetWorldManageState(character, connectionIn);

                character.FavoritedPawnIds = Server.Database.GetPawnFavorites(character.CharacterId, connectionIn);

                character.ExtendedParams = Server.Database.SelectOrbGainExtendParam(character.CommonId, connectionIn);
                if (character.ExtendedParams == null)
                {
                    // Old DB is in use and new table not populated with required data for character
                    Logger.Error($"Character: AccountId={character.AccountId}, CharacterId={character.CharacterId}, CommonId={character.CommonId} is missing table entry in 'ddon_orb_gain_extend_param'.");
                    return null;
                }

                character.ReleasedExtendedJobParams = Server.JobOrbUnlockManager.GetReleasedElements(character, connectionIn);
                Server.JobOrbUnlockManager.EvaluateJobOrbTreeUnlocks(character);

                character.EpitaphRoadState.UnlockedContent = Server.Database.GetEpitaphRoadUnlocks(character.CharacterId, connectionIn);
                if (Server.GameSettings.GameServerSettings.EnableEpitaphWeeklyRewards)
                {
                    character.EpitaphRoadState.WeeklyRewardsClaimed = Server.Database.GetEpitaphClaimedWeeklyRewards(character.CharacterId, connectionIn);
                }

                foreach (var jobId in Enum.GetValues(typeof(JobId)).Cast<JobId>())
                {
                    character.JobMasterReleasedElements[jobId] = Server.Database.GetJobMasterReleasedElements(character.CharacterId, jobId, connectionIn);
                    character.JobMasterActiveOrders[jobId] = Server.JobMasterManager.GetJobMasterActiveOrders(character, jobId, connectionIn);
                }

                // Calculate everything upfront so we don't need to calculate it every time in vocation select/upgrade handler.
                character.AcquirableSkills = CalculateAcquirableSkills(character);
                character.AcquirableAbilities = CalculateAcquirableAbilities(character);

                UpdateCharacterExtendedParams(character);

                if (fetchPawns)
                {
                    SelectPawns(character, connectionIn);
                }

                return character;
            });
        }

        /**
         * @note It is probably more efficient to have thiese entries in the DB but
         * the migration tool doesn't currently load quest data, so this is a workaround
         * to ensure everything works on existing characters.
         */
        private HashSet<ContentsRelease> GetContentsReleased(Character character, DbConnection? connectionIn = null)
        {
            var contentsReleased = new HashSet<ContentsRelease>();
            
            // Generate list of unlocked content
            foreach (var completedQuest in character.CompletedQuests.Values.Where(x => (x.QuestType == QuestType.Main) || (x.QuestType == QuestType.Tutorial)))
            {
                var quest = QuestManager.GetQuestByQuestId(completedQuest.QuestId);
                if (quest == null)
                {
                    continue;
                }
                contentsReleased.UnionWith(quest.ContentsRelease.Select(x => x.ReleaseId).ToHashSet());
            }

            // Find quests being resumed which have contents released mid quest
            var allQuestsInProgress = Server.Database.GetQuestProgressByType(character.CommonId, QuestType.All, connectionIn)
                .Where(x => x.QuestType == QuestType.Main || x.QuestType == QuestType.Tutorial);
            foreach (var progess in allQuestsInProgress)
            {
                var quest = QuestManager.GetQuestByScheduleId(progess.QuestScheduleId);
                if (quest == null)
                {
                    continue;
                }
                contentsReleased.UnionWith(quest.GetPartialContentsReleaseList(progess.Step));
            }

            // TODO: Based on server settings add additional contents unlocked here
            // TODO: For example, unlock everything, unlock all jobs, etc.

            return contentsReleased;
        }

        private Dictionary<QuestId, List<QuestFlagInfo>> GetWorldManageState(Character character, DbConnection? connectionIn = null)
        {
            var result = new Dictionary<QuestId, List<QuestFlagInfo>>();
            
            foreach (var completedQuest in character.CompletedQuests.Values.Where(x => (x.QuestType == QuestType.Main) || (x.QuestType == QuestType.Tutorial)))
            {
                var quest = QuestManager.GetQuestByQuestId(completedQuest.QuestId);
                if (quest == null)
                {
                    continue;
                }

                foreach (var (questId, flagList) in quest.WorldManageUnlocks)
                {
                    if (!result.ContainsKey(questId))
                    {
                        result[questId] = new List<QuestFlagInfo>();
                    }
                    result[questId].AddRange(flagList);
                }
            }

            return result;
        }

        private bool HasRequiredTraining(JobId jobId, ReleaseType releaseType, uint releaseId, uint releaseLevel)
        {
            if (!Server.AssetRepository.JobMasterAsset.JobOrders[jobId][releaseType].ContainsKey(releaseId))
            {
                return false;
            }

            return Server.AssetRepository.JobMasterAsset.JobOrders[jobId][releaseType][releaseId]
                .Where(x => x.ReleaseType == releaseType)
                .Where(x => x.ReleaseLv == releaseLevel)
                .Where(x => x.ReleaseId == releaseId)
                .Any();
        }

        private Dictionary<JobId, List<CDataAbilityParam>> CalculateAcquirableAbilities(Character character)
        {
            var acquirableAbilities = new Dictionary<JobId, List<CDataAbilityParam>>();

            foreach (var jobId in Enum.GetValues(typeof(JobId)).Cast<JobId>())
            {
                if (jobId == JobId.None)
                {
                    continue;
                }

                var skillData = new List<CDataAbilityParam>();
                foreach (var ability in SkillData.AllAbilities.Where(x => x.Job == jobId).ToList())
                {
                    var unlock = new CDataAbilityParam()
                    {
                        AbilityNo = ability.AbilityNo,
                        Job = ability.Job,
                        Type = ability.Type,
                    };

                    foreach (var abilityLevel in ability.Params)
                    {
                        bool isRelease;
                        if (!HasRequiredTraining(jobId, ReleaseType.Augment, ability.AbilityNo, abilityLevel.Lv) &&
                            !SkillData.IsUnlockableAbility(jobId, ability.AbilityNo, abilityLevel.Lv))
                        {
                            // The skill level has no unlock requirements
                            isRelease = true;
                        }
                        else if (SkillData.IsUnlockableAbility(ability.Job, ability.AbilityNo, abilityLevel.Lv))
                        {
                            isRelease = character.UnlockedAbilities[ability.Job].Contains(ability.AbilityNo);
                        }
                        else
                        {
                            // The augment level has a job training unlock requirement, so let's see if we unlocked it
                            isRelease = character.JobMasterReleasedElements[jobId]
                                .Where(x => x.ReleaseType == ReleaseType.Augment)
                                .Where(x => x.ReleaseLv == abilityLevel.Lv)
                                .Where(x => x.ReleaseId == ability.AbilityNo)
                                .Any();
                        }

                        unlock.Params.Add(new CDataAbilityLevelParam()
                        {
                            Lv = abilityLevel.Lv,
                            RequireJobLevel = abilityLevel.RequireJobLevel,
                            RequireJobPoint = abilityLevel.RequireJobPoint,
                            IsRelease = isRelease
                        });
                    }
                    skillData.Add(unlock);
                }

                if (!acquirableAbilities.ContainsKey(jobId))
                {
                    acquirableAbilities[jobId] = new List<CDataAbilityParam>();
                }
                acquirableAbilities[jobId].AddRange(skillData);
            }

            return acquirableAbilities;
        }

        private Dictionary<JobId, List<CDataSkillParam>> CalculateAcquirableSkills(Character character)
        {
            var acquirableSkills = new Dictionary<JobId, List<CDataSkillParam>>();

            foreach (var jobId in Enum.GetValues(typeof(JobId)).Cast<JobId>())
            {
                if (jobId == JobId.None)
                {
                    continue;
                }

                var skillData = new List<CDataSkillParam>();
                foreach (var skill in SkillData.AllSkills.Where(x => x.Job == jobId).ToList())
                {
                    var unlock = new CDataSkillParam()
                    {
                        SkillNo = skill.SkillNo,
                        Job = skill.Job,
                        Type = skill.Type,
                    };

                    foreach (var skillLevel in skill.Params)
                    {
                        bool isRelease;
                        if (!HasRequiredTraining(jobId, ReleaseType.CustomSkill, skill.SkillNo, skillLevel.Lv) &&
                            !SkillData.IsEm4Skill(jobId, skill.SkillNo, skillLevel.Lv) &&
                            !SkillData.IsUnlockableSkill(jobId, skill.SkillNo, skillLevel.Lv))
                        {
                            // The skill level has no unlock requirements
                            isRelease = true;
                        }
                        else if (SkillData.Em4CustomSkills.ContainsKey(jobId) && SkillData.IsEm4Skill(jobId, skill.SkillNo, skillLevel.Lv))
                        {
                            // The skill has an unlock requirement on EM4
                            isRelease = character.HasQuestCompleted(QuestId.TheShiningGate);
                        }
                        else if (SkillData.IsUnlockableSkill(skill.Job, skill.SkillNo, skillLevel.Lv))
                        {
                            isRelease = character.UnlockedCustomSkills[skill.Job].Contains(skill.SkillNo);
                        }
                        else
                        {
                            // The skill level has a job training unlock requirement, so let's see if we unlocked it
                            isRelease = character.JobMasterReleasedElements[jobId]
                                .Where(x => x.ReleaseType == ReleaseType.CustomSkill)
                                .Where(x => x.ReleaseLv == skillLevel.Lv)
                                .Where(x => x.ReleaseId == skill.SkillNo)
                                .Any();
                        }

                        unlock.Params.Add(new CDataSkillLevelParam()
                        {
                            Lv = skillLevel.Lv,
                            RequireJobLevel = skillLevel.RequireJobLevel,
                            RequireJobPoint = skillLevel.RequireJobPoint,
                            IsRelease = isRelease
                        });
                    }
                    skillData.Add(unlock);
                }

                if (!acquirableSkills.ContainsKey(jobId))
                {
                    acquirableSkills[jobId] = new List<CDataSkillParam>();
                }
                acquirableSkills[jobId].AddRange(skillData);
            }

            return acquirableSkills;
        }

        private void SelectPawns(Character character, DbConnection? connectionIn = null)
        {
            character.Pawns = Server.Database.SelectPawnsByCharacterId(character.ContentCharacterId, connectionIn);

            for (int i = 0; i < character.Pawns.Count; i++)
            {
                Pawn pawn = character.Pawns[i];
                pawn.Server = character.Server;
                pawn.Equipment = character.Storage.GetPawnEquipment(i);
                pawn.ExtendedParams = Server.Database.SelectOrbGainExtendParam(pawn.CommonId, connectionIn);
                pawn.EmblemStatList = Server.JobEmblemManager.GetEmblemStatsForCurrentJob(character, pawn.Job);
                if (pawn.ExtendedParams == null)
                {
                    // Old DB is in use and new table not populated with required data for character
                    Logger.Error($"Character: AccountId={character.AccountId}, CharacterId={character.ContentCharacterId}, CommonId={character.CommonId}, PawnCommonId={pawn.CommonId} is missing table entry in 'ddon_orb_gain_extend_param'.");
                }
                if (pawn.PawnType != PawnType.Main)
                {
                    Logger.Error($"Character: AccountId={character.AccountId}, CharacterId={character.ContentCharacterId}, CommonId={character.CommonId}, PawnCommonId={pawn.CommonId} has invalid pawn type; locally setting pawn type back to Main.");
                    pawn.PawnType = PawnType.Main;
                }

                UpdateCharacterExtendedParams(pawn, ownerCharacter: character);
            }
        }

        public void UpdateOnlineStatus(GameClient client, Character character, OnlineStatus onlineStatus)
        {
            client.Character.OnlineStatus = onlineStatus;
            var charUpdateNtc = new S2CCharacterCommunityCharacterStatusUpdateNtc();
            charUpdateNtc.UpdateCharacterList.Add(ContactListManager.CharacterToListEml(client.Character));
            charUpdateNtc.UpdateMatchingProfileList.Add(new CDataUpdateMatchingProfileInfo()
            {
                CharacterId = client.Character.CharacterId,
                Comment = client.Character.MatchingProfile.Comment,
            });

            // TODO: Is there a reduced set of clients we can send this to?
            foreach (var memberClient in Server.ClientLookup.GetAll())
            {
                memberClient.Send(charUpdateNtc);
            }
        }

        public uint GetMaxAugmentAllocation(CharacterCommon character)
        {
            return CharacterManager.BASE_ABILITY_COST_AMOUNT + character.ExtendedParams.AbilityCost;
        }

        public void UpdateCharacterExtendedParams(CharacterCommon characterCommon, bool newCharacter = false, Character ownerCharacter = null)
        {
            var extendedParams = characterCommon.ExtendedParams;

            // There is always an implicit + 1 ring slot plus the extended params value
            characterCommon.JewelrySlotNum = (byte)(CharacterManager.DEFAULT_RING_COUNT + extendedParams.JewelrySlot);

            /**
             * There are two physical attack traits and two magic attack traits in
             * the stats menu. This corresponds with the attack of the weapon and
             * the iniate attack of the character on the currently selected job.
             * Similar distinction made with magic. The Gain* stats are extra stats
             * on top of the iniate stats. These come from armors and BO/HO trees.
             */
            characterCommon.StatusInfo.GainAttack = extendedParams.Attack;
            characterCommon.StatusInfo.GainDefense = extendedParams.Defence;
            characterCommon.StatusInfo.GainMagicAttack = extendedParams.MagicAttack;
            characterCommon.StatusInfo.GainMagicDefense = extendedParams.MagicDefence;
            characterCommon.StatusInfo.GainStamina = extendedParams.StaminaMax;
            characterCommon.StatusInfo.GainHP = extendedParams.HpMax;

            /**
             * Additional stats can be earned from the S2 and S3 BO/HO orb trees.
             * The stat boosts rewarded for this mechanism rewards both stats for all jobs and stats for
             * a specific job only. We abuse JobId.None to store the stats for all jobs.
             */
            var extendedJobParams = characterCommon.ExtendedJobParams;

            JobId jobId = characterCommon.ActiveCharacterJobData?.Job ?? JobId.None;
            if (jobId == JobId.None)
            {
                Logger.Error($"Character CommonId {characterCommon.CommonId} has no active job data.");
                return;
            }

            if (characterCommon is Pawn)
            {
                extendedJobParams = ownerCharacter.ExtendedJobParams;
            }

            characterCommon.StatusInfo.GainAttack += (uint)(extendedJobParams[JobId.None].Attack + extendedJobParams[jobId].Attack);
            characterCommon.StatusInfo.GainDefense += (uint)(extendedJobParams[JobId.None].Defence + extendedJobParams[jobId].Defence);
            characterCommon.StatusInfo.GainMagicAttack += (uint)(extendedJobParams[JobId.None].MagicAttack + extendedJobParams[jobId].MagicAttack);
            characterCommon.StatusInfo.GainMagicDefense += (uint)(extendedJobParams[JobId.None].MagicDefence + extendedJobParams[jobId].MagicDefence);
            characterCommon.StatusInfo.GainStamina += (uint)(extendedJobParams[JobId.None].StaminaMax + extendedJobParams[jobId].StaminaMax);
            characterCommon.StatusInfo.GainHP += (uint)(extendedJobParams[JobId.None].HpMax + extendedJobParams[jobId].HpMax);

            /**
             * Seems when the game first loads, the game wants MaxHP to always be 760
             * and MaxStamina to be 450. Then it takes the values from the GainHp and
             * GainStamina and add them to the Max values. Finally it seems to take
             * the stats from the armor/accessories and add them to the running total
             * for each stat, resulting the stats you see in game.
             *
             * Later on when upgrading health at the dragon, if we leave these as
             * the default, the health will get adjusted back down. One thing we
             * can take advantage of is that if we set the character HP > MaxHP,
             * it will only fill up to max HP. This will allow us to refill the
             * health of the player when they upgrade with BO or in other
             * scenarios where this may be required. The same trick also works
             * for stamina.
             */
            if (characterCommon.StatusInfo.MaxHP != 0 || newCharacter)
            {
                characterCommon.StatusInfo.HP = CharacterManager.MAX_PLAYER_HP;
                characterCommon.StatusInfo.WhiteHP = CharacterManager.MAX_PLAYER_HP;
            }
            characterCommon.StatusInfo.MaxHP = CharacterManager.BASE_HEALTH;

            if (characterCommon.StatusInfo.MaxStamina != 0 || newCharacter)
            {
                characterCommon.StatusInfo.Stamina = CharacterManager.MAX_PLAYER_STAMINA;
            }
            characterCommon.StatusInfo.MaxStamina = CharacterManager.BASE_STAMINA;
        }

        public void CleanupOnExit(GameClient client)
        {
            // Cancel any pending timers
            Server.PartnerPawnManager.HandleLeaveFromParty(client);

            // Update player health in the DB
            UpdateDatabaseOnExit(client.Character);
        }

        private void UpdateDatabaseOnExit(Character character)
        {
            // When the character is first logging in, the HP
            // values are set to 0. If the player disconnects
            // before fully logging in, this handler will save
            // a value of 0 HP into the database. The next time
            // the player logs in, they will have no health causing
            // the game to function improperly.
            if (character.GreenHp == 0 || character.WhiteHp == 0)
            {
                return;
            }

            Server.Database.UpdateStatusInfo(character);

            foreach (var pawn in character.Pawns)
            {
                // Reset pawn HP to base max so next time we log in they are at full health
                pawn.GreenHp = CharacterManager.BASE_HEALTH;
                pawn.WhiteHp = CharacterManager.BASE_HEALTH;

                Server.Database.UpdateStatusInfo(pawn);
            }
        }

        public PacketQueue UpdateCharacterExtendedParamsNtc(GameClient client, CharacterCommon character)
        {
            UpdateCharacterExtendedParams(character, ownerCharacter: client.Character);
            return NotifyClientOfCharacterStatus(client, character);
        }

        private PacketQueue NotifyClientOfCharacterStatus(GameClient client, CharacterCommon characterCommon)
        {
            PacketQueue queue = new();

            if (characterCommon is Character)
            {
                S2CContextGetLobbyPlayerContextNtc ntc1 = new S2CContextGetLobbyPlayerContextNtc();
                GameStructure.S2CContextGetLobbyPlayerContextNtc(Server, ntc1, (Character) characterCommon);

                S2CExtendEquipSlotNtc ntc2 = new S2CExtendEquipSlotNtc()
                {
                    EquipSlot = EquipCategory.Jewelry,
                    AddNum = 0,
                    TotalNum = characterCommon.JewelrySlotNum
                };

                if (client.Party != null)
                {
                    client.Party.EnqueueToAll(ntc1, queue);
                }
                else
                {
                    client.Enqueue(ntc1, queue);
                }

                client.Enqueue(ntc2, queue);
            }
            else
            {
                PartyMember partyMember = client.Party.GetPartyMemberByCharacter(characterCommon);
                if (partyMember == null || partyMember is not PawnPartyMember)
                {
                    Logger.Error($"Failed to find party member in the list");
                    return queue;
                }

                PawnPartyMember pawnPartyMember = (PawnPartyMember)partyMember;
                if (client.Party != null)
                {
                    client.Party.EnqueueToAll(pawnPartyMember.GetPartyContext(), queue);
                }
                else
                {
                    // This should never be true but if it is, why?
                    client.Enqueue(pawnPartyMember.GetPartyContext(), queue);
                }
            }

            return queue;
        }

        public void UnlockCustomSkill(Character character, JobId jobId, uint releaseId, uint releaseLevel)
        {
            var acquireableSkill = character.AcquirableSkills[jobId]
                .Where(x => x.SkillNo == releaseId)
                .SelectMany(x => x.Params)
                .Where(x => x.Lv == releaseLevel)
                .FirstOrDefault() ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_SKILL_PARAM_NOT_FOUND, $"Failed to locate the custom skill to unlock {jobId}:{releaseId}:{releaseLevel}");
            acquireableSkill.IsRelease = true;
            character.UnlockedCustomSkills[jobId].Add(releaseId);
        }

        public void UnlockAbility(Character character, JobId jobId, uint releaseId, uint releaseLevel)
        {
            var acquireableAbility = character.AcquirableAbilities[jobId]
                .Where(x => x.AbilityNo == releaseId)
                .SelectMany(x => x.Params)
                .Where(x => x.Lv == releaseLevel)
                .FirstOrDefault() ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_SKILL_PARAM_NOT_FOUND, $"Failed to locate the augment to unlock {jobId}:{releaseId}:{releaseLevel}");
            acquireableAbility.IsRelease = true;
            character.UnlockedAbilities[jobId].Add(releaseId);
        }
    }
}
