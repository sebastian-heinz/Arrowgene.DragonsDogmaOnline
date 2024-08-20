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
         * @brief CharacterId is used by the client to identify different players in the game.
         * To support other game modes, such as Bitterblack Maze, internally the server creates
         * a second character as a majority of the tables overlap. When we switch to the BBM
         * game mode, it still thinks we are the original character we logged in as. This is where
         * NormalCharacterId and BbmCharacterId come into play. Certain systems such as the party,
         * wallet and equipment systems require the original character ID regardless of the gamemode.
         * In this case we would use directly the variable "NormalCharacterId". For everything else, we
         * can use CharacterId directly. Then depending on the GameMode it will return either the
         * CharacterId for the normal mode of play or the CharacterId for BBM related operations.
         */
        public uint CharacterId
        {
            get
            {
                if (BbmCharacterId != 0)
                {
                    return this.BbmCharacterId;
                }
                else
                {
                    return this.NormalCharacterId;
                }
            }
            set
            {
                this.NormalCharacterId = value;
            }
        }

        public uint NormalCharacterId;
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
