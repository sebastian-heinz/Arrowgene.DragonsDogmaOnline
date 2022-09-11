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
            PlayPointList = new List<CDataJobPlayPoint>();
            CharacterEquipDataListDictionary = new Dictionary<JobId, List<CDataCharacterEquipData>>();
            CharacterEquipViewDataListDictionary = new Dictionary<JobId, List<CDataCharacterEquipData>>();
            CharacterEquipJobItemListDictionary = new Dictionary<JobId, List<CDataEquipJobItem>>();
            CharacterItemSlotInfoList = new List<CDataCharacterItemSlotInfo>();
            UnkCharData0 = new List<UnknownCharacterData0>();
            UnkCharData1 = new List<UnknownCharacterData1>(); // Currencies? 1 is G, 2 is RP...
            OrbStatusList = new List<CDataOrbPageStatus>();
            MsgSetList = new List<CDataCharacterMsgSet>();
            ShortCutList = new List<CDataShortCut>();
            CommunicationShortCutList = new List<CDataCommunicationShortCut>();
            MatchingProfile = new CDataMatchingProfile();
            ArisenProfile = new CDataArisenProfile();
            NormalSkills = new List<CDataNormalSkillParam>();
            CustomSkills = new List<CDataSetAcquirementParam>();
            Abilities = new List<CDataSetAcquirementParam>();
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
        public Dictionary<JobId, List<CDataCharacterEquipData>> CharacterEquipDataListDictionary;
        public Dictionary<JobId, List<CDataCharacterEquipData>> CharacterEquipViewDataListDictionary;
        public Dictionary<JobId, List<CDataEquipJobItem>> CharacterEquipJobItemListDictionary;
        public byte JewelrySlotNum;
        public List<CDataCharacterItemSlotInfo> CharacterItemSlotInfoList;

        // One of these is CDataWalletPoint, can't determine which.
        public List<UnknownCharacterData0> UnkCharData0;
        public List<UnknownCharacterData1> UnkCharData1;
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
        public List<CDataSetAcquirementParam> CustomSkills { get; set;}
        public List<CDataSetAcquirementParam> Abilities { get; set; }
        
        /// TODO combine into a location class ?
        public StageId Stage { get; set; }
        public uint StageNo { get; set; }
        public double X { get; set; }
        public float Y { get; set; }
        public double Z { get; set; }
        // ---
    }
}
