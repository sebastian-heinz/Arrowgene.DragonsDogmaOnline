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
            CharacterEquipItemListDictionary = new Dictionary<JobId, List<EquipItem>>();
            CharacterEquipViewItemListDictionary = new Dictionary<JobId, List<EquipItem>>();
            CharacterEquipJobItemListDictionary = new Dictionary<JobId, List<CDataEquipJobItem>>();
            Unk0 = new List<UnknownCharacterData0>();
            CharacterItemSlotInfoList = new List<CDataCharacterItemSlotInfo>();
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
            Items = new Dictionary<StorageType, List<EquipItem>>() {
                // Taken from pcaps
                { StorageType.ItemBagConsumable, new List<EquipItem>(Enumerable.Repeat<EquipItem>(null, 20)) },
                { StorageType.ItemBagMaterial, new List<EquipItem>(Enumerable.Repeat<EquipItem>(null, 40)) },
                { StorageType.ItemBagEquipment, new List<EquipItem>(Enumerable.Repeat<EquipItem>(null, 40)) },
                { StorageType.ItemBagJob, new List<EquipItem>(Enumerable.Repeat<EquipItem>(null, 5)) },
                { StorageType.Unk5, new List<EquipItem>(Enumerable.Repeat<EquipItem>(null, 20)) },
                { StorageType.StorageBoxNormal, new List<EquipItem>(Enumerable.Repeat<EquipItem>(null, 400)) },
                { StorageType.Unk7, new List<EquipItem>(Enumerable.Repeat<EquipItem>(null, 400)) },
                { StorageType.Unk8, new List<EquipItem>(Enumerable.Repeat<EquipItem>(null, 400)) },
                { StorageType.Unk9, new List<EquipItem>(Enumerable.Repeat<EquipItem>(null, 400)) },
                { StorageType.Unk10, new List<EquipItem>(Enumerable.Repeat<EquipItem>(null, 400)) },
                { StorageType.Unk11, new List<EquipItem>(Enumerable.Repeat<EquipItem>(null, 0)) },
                { StorageType.Unk12, new List<EquipItem>(Enumerable.Repeat<EquipItem>(null, 800)) },
                { StorageType.Unk13, new List<EquipItem>(Enumerable.Repeat<EquipItem>(null, 400)) },
                { StorageType.Unk14, new List<EquipItem>(Enumerable.Repeat<EquipItem>(null, 0)) },
                { StorageType.Unk15, new List<EquipItem>(Enumerable.Repeat<EquipItem>(null, 0)) },
                { StorageType.Unk16, new List<EquipItem>(Enumerable.Repeat<EquipItem>(null, 0)) },
                { StorageType.Unk17, new List<EquipItem>(Enumerable.Repeat<EquipItem>(null, 0)) }
            };
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
        public Dictionary<JobId, List<EquipItem>> CharacterEquipItemListDictionary;
        public Dictionary<JobId, List<EquipItem>> CharacterEquipViewItemListDictionary;
        public Dictionary<JobId, List<CDataEquipJobItem>> CharacterEquipJobItemListDictionary;
        public byte JewelrySlotNum;
        public List<UnknownCharacterData0> Unk0;
        public List<CDataCharacterItemSlotInfo> CharacterItemSlotInfoList;
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
        public Dictionary<StorageType, List<EquipItem>> Items { get; set; } // TODO: Use its own thing and not EquipItem
        
        /// TODO combine into a location class ?
        public StageId Stage { get; set; }
        public uint StageNo { get; set; }
        public double X { get; set; }
        public float Y { get; set; }
        public double Z { get; set; }
        // ---

        public CDataGameServerListInfo Server;
    }
}
