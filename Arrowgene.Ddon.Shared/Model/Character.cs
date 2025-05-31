using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.BattleContent;
using Arrowgene.Ddon.Shared.Model.Clan;
using Arrowgene.Ddon.Shared.Model.EpitaphRoad;
using Arrowgene.Ddon.Shared.Model.Quest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.Shared.Model
{
    public class Character : CharacterCommon
    {
        public Character() : base()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Created = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
            PlayPointList = new List<CDataJobPlayPoint>();
            Storage = new Storages(new Dictionary<StorageType, ushort>());
            WalletPointList = new List<CDataWalletPoint>();
            OrbStatusList = new List<CDataOrbPageStatus>();
            MsgSetList = new List<CDataCharacterMsgSet>();
            ShortCutList = new List<CDataShortCut>();
            CommunicationShortCutList = new List<CDataCommunicationShortCut>();
            MatchingProfile = new CDataMatchingProfile();
            ArisenProfile = new CDataArisenProfile();
            Pawns = new List<Pawn>();
            RentedPawns = new List<Pawn>();
            ReleasedWarpPoints = new List<ReleasedWarpPoint>();
            OnlineStatus = OnlineStatus.Offline;
            ContextOwnership = new Dictionary<ulong, bool>();
            StampBonus = new CharacterStampBonus();
            AbilityPresets = new List<CDataPresetAbilityParam>();
            BinaryData = new byte[C2SBinarySaveSetCharacterBinSaveDataReq.ARRAY_SIZE];
            LastSeenLobby = new Dictionary<uint, uint>();
            BbmProgress = new BitterblackMazeProgress();
            CompletedQuests = new Dictionary<QuestId, CompletedQuest>();
            ClanName = new ClanName();
            EpitaphRoadState = new EpitaphRoadState();
            AreaRanks = new();
            AreaSupply = new();
            AchievementProgress = new();
            AchievementStatus = new();
            AchievementUniqueCrafts = new();

            UnlockableItems = new();

            PartnerTimerLockObj = new();
            ContentsReleased = new HashSet<ContentsRelease>();
            WorldManageUnlocks = new Dictionary<QuestId, List<QuestFlagInfo>>();

            JobMasterReleasedElements = new();
            JobMasterActiveOrders = new();
            AcquirableSkills = new();
            AcquirableAbilities = new();

            JobEmblems = new();
        }

        public int AccountId { get; set; }
        public DateTime Created { get; set; }

        /**
         * @brief ContentCharacter is used when there is an alias mapping between one
         * "main character id" (the one the player logs in as) and a particular content or gamemode.
         * To support other game modes, such as Bitterblack Maze, internally the server creates
         * a second character as a majority of the tables overlap. When we switch to the BBM
         * game mode, it still thinks we are the original character we logged in as. This is where
         * ContentCharacterId comes into play. Paritculary we need this for Database operations
         * when selecting between different tables depending on the game mode.
         */
        public uint ContentCharacterId
        {
            get
            {
                if (this.GameMode == GameMode.BitterblackMaze)
                {
                    return this.BbmCharacterId;
                }
                else
                {
                    return this.CharacterId;
                }
            }
        }

        public uint CharacterId { get; set; }
        public uint BbmCharacterId { get; set; }
        public uint UserId { get; set; }
        public uint Version { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<CDataJobPlayPoint> PlayPointList { get; set; }
        public Storages Storage { get; set; }
        public List<CDataWalletPoint> WalletPointList { get; set; }
        public QuestAreaId AreaId { get; set; }

        /// <summary>
        /// num = 0: New character
        /// num = 1: Complete the main quest "A Servant's Pledge"
        /// num = 2: Riftstone Shards×10, Available in each area's supplies
        /// num = 3: Dragonforce Augmentation, Obtain "Master Vessel +1", ×10 Riftstone Shards, "3rd Level of Dragon Power" ○ Main Quest "The Whereabouts of Life" ○ Total Level of the Master is 120 or more ○ Required BO for the Master: 19370 (3rd level)
        /// </summary>
        public byte MyPawnSlotNum { get; set; }
        /// <summary>
        /// Support Pawns can join a party as long as there is at least one main Pawn in the party. (e.g. 1 main pawn + 2 support pawns)
        /// num = 0: New character
        /// num = 3: Complete the main quest "A Servant's Pledge"
        /// num = 4: Dragonforce Augmentation Obtain "Vessel of Leadership +1" ← "2nd Level of Dragon Power" ○ Complete the main Quest "Awakening of the Gods" ○ Total Level of the Master is 70 or more ○ Required BO for the Master: 4990 (2nd level)
        /// num = 5: Dragonforce Augmentation Obtain "Commander's Vessel +1" ← "4th Level of Dragon Power" ○ Complete the main quest "White Dragon, Be Eternal" ○ The total level of the Master is 160 or more ○ Required BO for the Master: 70100 (4th level)
        /// </summary>
        public byte RentalPawnSlotNum { get; set; }
        public List<CDataOrbPageStatus> OrbStatusList { get; set; }
        public List<CDataCharacterMsgSet> MsgSetList { get; set; }
        public List<CDataShortCut> ShortCutList { get; set; }
        public List<CDataCommunicationShortCut> CommunicationShortCutList { get; set; }
        public CDataMatchingProfile MatchingProfile { get; set; }
        public CDataArisenProfile ArisenProfile { get; set; }
        public bool HideEquipHeadPawn { get; set; }
        public bool HideEquipLanternPawn { get; set; }
        public byte ArisenProfileShareRange { get; set; }
        public List<CDataPresetAbilityParam> AbilityPresets { get; set; }
        public byte[] BinaryData { get; set; }
        public GameMode GameMode {  get; set; }
        public Dictionary<uint, uint> LastSeenLobby { get; set; }
        public uint PartnerPawnId { get; set; }
        public uint PartnerPawnAdventureTimerId { get; set; }
        public object PartnerTimerLockObj { get; set; }
        public List<Pawn> Pawns { get; set; }
        public List<Pawn> RentedPawns {  get; set; }
        public HashSet<uint> FavoritedPawnIds { get; set; } = new();
        public uint FavWarpSlotNum { get; set; }
        public List<ReleasedWarpPoint> ReleasedWarpPoints { get; set; }
        public BitterblackMazeProgress BbmProgress;
        public uint NextBBMStageId {  get; set; }
        public uint MaxBazaarExhibits { get; set; }
        public Dictionary<QuestId, CompletedQuest> CompletedQuests { get; set; }
        public uint LastSafeStageId { get; set; }
        public uint ClanId { get; set; }
        public ClanName ClanName { get; set; }
        public bool IsLanternLit { get; set; }
        public uint LanternTimer { get; set; }

        public Dictionary<JobId, JobEmblem> JobEmblems { get; set; }

        public EpitaphRoadState EpitaphRoadState { get; set; }
        public Dictionary<QuestAreaId, AreaRank> AreaRanks { get; set; }
        public Dictionary<QuestAreaId, List<CDataRewardItemInfo>> AreaSupply { get; set; }

        public Dictionary<(AchievementType, uint), uint> AchievementProgress { get; set; }
        public Dictionary<uint, DateTimeOffset> AchievementStatus { get; set; }
        public Dictionary<AchievementCraftTypeParam, HashSet<ItemId>> AchievementUniqueCrafts { get; set; }

        public HashSet<(UnlockableItemCategory Category, uint Id)> UnlockableItems { get; set; }

        public HashSet<ContentsRelease> ContentsReleased { get; set; }
        public Dictionary<QuestId, List<QuestFlagInfo>> WorldManageUnlocks { get; set; }

        public Dictionary<JobId, List<CDataReleaseElement>> JobMasterReleasedElements { get; set; }
        public Dictionary<JobId, List<CDataActiveJobOrder>> JobMasterActiveOrders { get; set; }
        public Dictionary<JobId, List<CDataSkillParam>> AcquirableSkills { get; set; }
        public Dictionary<JobId, List<CDataAbilityParam>> AcquirableAbilities { get; set; }

        // TODO: Move to a more sensible place
        public uint LastEnteredShopId { get; set; }

        public Pawn PawnBySlotNo(byte slotNo)
        {
            if (slotNo == 0 || slotNo > Pawns.Count)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_INVALID_SLOT_NO,
                    $"Requesting invalid main pawn slot {slotNo} for character {CharacterId}");
            }

            return Pawns[slotNo - 1];
        }

        public Pawn RentedPawnBySlotNo(byte slotNo)
        {
            if (slotNo > RentedPawns.Count)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_INVALID_SLOT_NO,
                    $"Requesting invalid rented slot {slotNo} for character {CharacterId}");
            }

            return RentedPawns[slotNo - 1];
        }

        public void RemoveRentedPawnBySlotNo(byte slotNo)
        {
            if (slotNo > RentedPawns.Count)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_INVALID_SLOT_NO,
                    $"Removing invalid rented slot {slotNo} for character {CharacterId}");
            }

            RentedPawns.RemoveAt(slotNo - 1);
        }

        public Pawn PawnById(uint pawnId, PawnType type = PawnType.None)
        {
            switch (type)
            {
                case PawnType.Main:
                    var mainPawn = Pawns.Where(x => x.PawnId == pawnId).FirstOrDefault();
                    if (mainPawn is not null)
                    {
                        return mainPawn;
                    }
                    break;
                case PawnType.Support:
                    var rentalPawn = RentedPawns.Where(x => x.PawnId == pawnId).FirstOrDefault();
                    if (rentalPawn is not null)
                    {
                        return rentalPawn;
                    }
                    break;
            }

            throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_NOT_FOUNDED, $"Could not find pawn with ID {pawnId}, type {type}");
        }

        public Dictionary<ulong, bool> ContextOwnership { get; set; }

        public CDataJobPlayPoint? ActiveCharacterPlayPointData
        {
            get { return PlayPointList.Where(x => x.Job == Job).SingleOrDefault(); }
        }

        public CharacterStampBonus StampBonus { get; set; }

        public CDataCommunityCharacterBaseInfo GetCommunityCharacterBaseInfo()
        {
            return new CDataCommunityCharacterBaseInfo
            {
                CharacterId = CharacterId,
                CharacterName = new CDataCharacterName
                {
                    FirstName = FirstName,
                    LastName = LastName,
                },
                ClanName = ClanName.ShortName
            };
        }

        public List<CDataCharacterReleaseElement> GetReleasedContent()
        {
            return ContentsReleased.Select(x => x.ToCDataCharacterReleaseElement()).ToList();
        }

        public bool HasContentReleased(ContentsRelease releaseId)
        {
            return ContentsReleased.Contains(releaseId);
        }

        public List<CDataQuestFlag> GetWorldManageQuestUnlocks(QuestId questId)
        {
            if (!WorldManageUnlocks.ContainsKey(questId))
            {
                return new();
            }

            return WorldManageUnlocks[questId]
                .Where(x => x.FlagType == QuestFlagType.WorldManageQuest)
                .Select(x => new CDataQuestFlag() { FlagId = x.Value })
                .ToList();
        }

        public List<CDataQuestLayoutFlag> GetWorldManageLayoutUnlocks(QuestId questId)
        {
            if (!WorldManageUnlocks.ContainsKey(questId))
            {
                return new();
            }

            return WorldManageUnlocks[questId]
                .Where(x => x.FlagType == QuestFlagType.WorldManageLayout)
                .Select(x => new CDataQuestLayoutFlag() { FlagId = x.Value })
                .ToList();
        }

        public bool HasQuestCompleted(QuestId questId)
        {
            return CompletedQuests.ContainsKey(questId);
        }

        public bool HasJobOfLevel(JobId jobId, uint level)
        {
            return CharacterJobDataList.Any(x => x.Job == jobId && x.Lv >= level);
        }
    }
}
