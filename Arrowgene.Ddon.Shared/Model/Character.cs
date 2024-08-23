using System;
using System.Linq;
using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.BattleContent;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

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
            CharacterEquipItemInfoUnk2 = new List<CDataEquipItemInfoUnk2>();
            WalletPointList = new List<CDataWalletPoint>();
            OrbStatusList = new List<CDataOrbPageStatus>();
            MsgSetList = new List<CDataCharacterMsgSet>();
            ShortCutList = new List<CDataShortCut>();
            CommunicationShortCutList = new List<CDataCommunicationShortCut>();
            MatchingProfile = new CDataMatchingProfile();
            ArisenProfile = new CDataArisenProfile();
            Pawns = new List<Pawn>();
            ReleasedWarpPoints = new List<ReleasedWarpPoint>();
            OnlineStatus = OnlineStatus.Offline;
            ContextOwnership = new Dictionary<ulong, bool>();
            StampBonus = new CharacterStampBonus();
            AbilityPresets = new List<CDataPresetAbilityParam>();
            BinaryData = new byte[C2SBinarySaveSetCharacterBinSaveDataReq.ARRAY_SIZE];
            LastSeenLobby = new Dictionary<uint, uint>();
            BbmProgress = new BitterblackMazeProgress();
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

        public uint CharacterId;
        public uint BbmCharacterId;
        public uint UserId;
        public uint Version;
        public string FirstName;
        public string LastName;
        public List<CDataJobPlayPoint> PlayPointList;
        public Storages Storage;
        public List<CDataEquipItemInfoUnk2> CharacterEquipItemInfoUnk2;
        public List<CDataWalletPoint> WalletPointList;
        public byte MyPawnSlotNum;
        public byte RentalPawnSlotNum;
        public List<CDataOrbPageStatus> OrbStatusList;
        public List<CDataCharacterMsgSet> MsgSetList;
        public List<CDataShortCut> ShortCutList;
        public List<CDataCommunicationShortCut> CommunicationShortCutList;
        public CDataMatchingProfile MatchingProfile;
        public CDataArisenProfile ArisenProfile;
        public bool HideEquipHeadPawn;
        public bool HideEquipLanternPawn;
        public byte ArisenProfileShareRange;
        public List<CDataPresetAbilityParam> AbilityPresets;
        public byte[] BinaryData;
        public GameMode GameMode {  get; set; }

        public Dictionary<uint, uint> LastSeenLobby { get; set; }

        public List<Pawn> Pawns { get; set; }

        public uint FavWarpSlotNum { get; set; }
        public List<ReleasedWarpPoint> ReleasedWarpPoints { get; set; }

        public BitterblackMazeProgress BbmProgress;
        public uint NextBBMStageId {  get; set; }

        public uint MaxBazaarExhibits { get; set; }

        // ---

        // TODO: Move to a more sensible place
        public uint LastEnteredShopId { get; set; }

        public Pawn PawnBySlotNo(byte SlotNo)
        {
            return Pawns[SlotNo-1];
        }

        public Dictionary<ulong, bool> ContextOwnership { get; set; }

        public CDataJobPlayPoint? ActiveCharacterPlayPointData
        {
            get { return PlayPointList.Where(x => x.Job == Job).SingleOrDefault(); }
        }

        public CharacterStampBonus StampBonus { get; set; }
    }
}
