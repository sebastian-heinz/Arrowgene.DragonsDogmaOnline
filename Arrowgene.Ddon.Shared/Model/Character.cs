using System;
using System.Linq;
using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public class Character
    {
        public Character()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Created = DateTime.MinValue;
            EditInfo = new CDataEditInfo();
            StatusInfo = new CDataStatusInfo();
            CharacterJobDataList = new List<CDataCharacterJobData>();
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
            Equipment = new Equipment();
            CharacterEquipJobItemListDictionary = new Dictionary<JobId, List<CDataEquipJobItem>>();
            Unk0 = new List<UnknownCharacterData0>();
            WalletPointList = new List<CDataWalletPoint>();
            OrbStatusList = new List<CDataOrbPageStatus>();
            MsgSetList = new List<CDataCharacterMsgSet>();
            ShortCutList = new List<CDataShortCut>();
            CommunicationShortCutList = new List<CDataCommunicationShortCut>();
            MatchingProfile = new CDataMatchingProfile();
            ArisenProfile = new CDataArisenProfile();
            NormalSkills = new List<CDataNormalSkillParam>();
            CustomSkills = new List<CustomSkill>();
            Abilities = new List<Ability>();
            Pawns = new List<Pawn>();
        }

        public CDataCharacterJobData ActiveCharacterJobData
        {
            get { return CharacterJobDataList.Where(x => x.Job == Job).Single(); }
        }

        public int AccountId { get; set; }
        public DateTime Created { get; set; }
        public uint Id;
        public uint UserId;
        public uint Version;
        public string FirstName;
        public string LastName;
        public CDataEditInfo EditInfo;
        public CDataStatusInfo StatusInfo;
        public JobId Job;
        public List<CDataCharacterJobData> CharacterJobDataList;
        public List<CDataJobPlayPoint> PlayPointList;
        public Storages Storage;
        public Equipment Equipment;
        public Dictionary<JobId, List<CDataEquipJobItem>> CharacterEquipJobItemListDictionary;
        public byte JewelrySlotNum;
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
        public bool HideEquipHead;
        public bool HideEquipLantern;
        public bool HideEquipHeadPawn;
        public bool HideEquipLanternPawn;
        public byte ArisenProfileShareRange;

        public OnlineStatus OnlineStatus;
        public List<CDataNormalSkillParam> NormalSkills { get; set; }
        public List<CustomSkill> CustomSkills { get; set;}
        public List<Ability> Abilities { get; set; }

        public List<Pawn> Pawns { get; set; }
        
        /// TODO combine into a location class ?
        public StageId Stage { get; set; }
        public uint StageNo { get; set; }
        public double X { get; set; }
        public float Y { get; set; }
        public double Z { get; set; }
        // ---

        // TODO: Move to a more sensible place
        public uint LastEnteredShopId { get; set; }

        public CDataGameServerListInfo Server;
    }
}
