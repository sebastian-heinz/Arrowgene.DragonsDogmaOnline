using System;
using System.Linq;
using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public class Character : CharacterCommon
    {
        public Character():base()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Created = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
            PlayPointList = Enum.GetValues(typeof(JobId)).Cast<JobId>().Select(job => new CDataJobPlayPoint()
            {
                Job = job,
                PlayPoint = new CDataPlayPointData()
                {
                    ExpMode = 1, // EXP
                    PlayPoint = 0
                }
            }).ToList();
            Storage = new Storages(new Dictionary<StorageType, ushort>());
            Unk0 = new List<UnknownCharacterData0>();
            WalletPointList = new List<CDataWalletPoint>();
            OrbStatusList = new List<CDataOrbPageStatus>();
            MsgSetList = new List<CDataCharacterMsgSet>();
            ShortCutList = new List<CDataShortCut>();
            CommunicationShortCutList = new List<CDataCommunicationShortCut>();
            MatchingProfile = new CDataMatchingProfile();
            ArisenProfile = new CDataArisenProfile();
            Pawns = new List<Pawn>();
            ReleasedWarpPoints = new List<ReleasedWarpPoint>();
        }

        public int AccountId { get; set; }
        public DateTime Created { get; set; }
        public uint CharacterId;
        public uint UserId;
        public uint Version;
        public string FirstName;
        public string LastName;
        public List<CDataJobPlayPoint> PlayPointList;
        public Storages Storage;
        public List<UnknownCharacterData0> Unk0;
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

        public List<Pawn> Pawns { get; set; }

        public uint FavWarpSlotNum { get; set; }
        public List<ReleasedWarpPoint> ReleasedWarpPoints { get; set; }
        
        /// TODO combine into a location class ?
        public StageId Stage { get; set; }
        public uint StageNo { get; set; }
        public double X { get; set; }
        public float Y { get; set; }
        public double Z { get; set; }
        // ---

        // TODO: Move to a more sensible place
        public uint LastEnteredShopId { get; set; }

        public Pawn PawnBySlotNo(byte SlotNo)
        {
            return Pawns[SlotNo-1];
        }
    }
}
