using System.Collections.Generic;
using System.Linq;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCharacterInfo
    {
        public CDataCharacterInfo(Character character)
        {
            CharacterId = character.Id;
            UserId = character.UserId;
            Version = character.Version;
            FirstName = character.FirstName;
            LastName = character.LastName;
            EditInfo = character.EditInfo;
            StatusInfo = character.StatusInfo;
            Job = character.Job;
            CharacterJobDataList = character.CharacterJobDataList;
            PlayPointList = character.PlayPointList;
            CharacterEquipDataList = new List<CDataCharacterEquipData>() { new CDataCharacterEquipData() {
                    Equips = character.CharacterEquipItemListDictionary[character.Job]
                        .Select(x => x.AsCDataEquipItemInfo()).ToList()
            }};
            CharacterEquipViewDataList = new List<CDataCharacterEquipData>() { new CDataCharacterEquipData() {
                    Equips = character.CharacterEquipViewItemListDictionary[character.Job]
                        .Select(x => x.AsCDataEquipItemInfo()).ToList()
            }};
            CharacterEquipJobItemList = character.CharacterEquipJobItemListDictionary[character.Job];
            JewelrySlotNum = character.JewelrySlotNum;
            Unk0 = character.Unk0;
            CharacterItemSlotInfoList = character.CharacterItemSlotInfoList;
            WalletPointList = character.WalletPointList;
            MyPawnSlotNum = character.MyPawnSlotNum;
            RentalPawnSlotNum = character.RentalPawnSlotNum;
            OrbStatusList = character.OrbStatusList;
            MsgSetList = character.MsgSetList;
            ShortCutList = character.ShortCutList;
            CommunicationShortCutList = character.CommunicationShortCutList;
            MatchingProfile = character.MatchingProfile;
            ArisenProfile = character.ArisenProfile;
            HideEquipHead = character.HideEquipHead;
            HideEquipLantern = character.HideEquipLantern;
            HideEquipHeadPawn = character.HideEquipHeadPawn;
            HideEquipLanternPawn = character.HideEquipLanternPawn;
            ArisenProfileShareRange = character.ArisenProfileShareRange;
            OnlineStatus = character.OnlineStatus;
        }

        public CDataCharacterInfo()
        {
            CharacterId = 0;
            UserId = 0;
            Version = 0;
            FirstName = "";
            LastName = "";
            EditInfo = new CDataEditInfo();
            StatusInfo = new CDataStatusInfo();
            Job = 0;
            CharacterJobDataList = new List<CDataCharacterJobData>();
            PlayPointList = new List<CDataJobPlayPoint>();
            CharacterEquipDataList = new List<CDataCharacterEquipData>();
            CharacterEquipViewDataList = new List<CDataCharacterEquipData>();
            CharacterEquipJobItemList = new List<CDataEquipJobItem>();
            JewelrySlotNum = 0;
            Unk0 = new List<UnknownCharacterData0>();
            CharacterItemSlotInfoList = new List<CDataCharacterItemSlotInfo>(); // Slots in each Item Bag and Storage
            WalletPointList = new List<CDataWalletPoint>(); // Currencies? 1 is G, 2 is RP...
            MyPawnSlotNum = 0;
            RentalPawnSlotNum = 0;
            OrbStatusList = new List<CDataOrbPageStatus>();
            MsgSetList = new List<CDataCharacterMsgSet>();
            ShortCutList = new List<CDataShortCut>();
            CommunicationShortCutList = new List<CDataCommunicationShortCut>();
            MatchingProfile = new CDataMatchingProfile();
            ArisenProfile = new CDataArisenProfile();
            HideEquipHead = false;
            HideEquipLantern = false;
            HideEquipHeadPawn = false;
            HideEquipLanternPawn = false;
            ArisenProfileShareRange = 0;
            OnlineStatus = 0;
        }

        public uint CharacterId;
        public uint UserId;
        public uint Version;
        public string FirstName;
        public string LastName;
        public CDataEditInfo EditInfo;
        public CDataStatusInfo StatusInfo;
        public JobId Job;
        public List<CDataCharacterJobData> CharacterJobDataList;
        public List<CDataJobPlayPoint> PlayPointList;
        public List<CDataCharacterEquipData> CharacterEquipDataList;
        public List<CDataCharacterEquipData> CharacterEquipViewDataList;
        public List<CDataEquipJobItem> CharacterEquipJobItemList;
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
    }
    public class CDataCharacterInfoSerializer : EntitySerializer<CDataCharacterInfo>
    {
        public override void Write(IBuffer buffer, CDataCharacterInfo obj)
        {
            WriteUInt32(buffer, obj.CharacterId);
            WriteUInt32(buffer, obj.UserId);
            WriteUInt32(buffer, obj.Version);
            WriteMtString(buffer, obj.FirstName);
            WriteMtString(buffer, obj.LastName);
            WriteEntity(buffer, obj.EditInfo);
            WriteEntity(buffer, obj.StatusInfo);
            WriteByte(buffer, (byte) obj.Job);
            WriteEntityList(buffer, obj.CharacterJobDataList);
            WriteEntityList(buffer, obj.PlayPointList);
            WriteEntityList(buffer, obj.CharacterEquipDataList);
            WriteEntityList(buffer, obj.CharacterEquipViewDataList);
            WriteEntityList(buffer, obj.CharacterEquipJobItemList);
            WriteByte(buffer, obj.JewelrySlotNum);
            WriteEntityList(buffer, obj.Unk0);
            WriteEntityList(buffer, obj.CharacterItemSlotInfoList);
            WriteEntityList(buffer, obj.WalletPointList);
            WriteByte(buffer, obj.MyPawnSlotNum);
            WriteByte(buffer, obj.RentalPawnSlotNum);
            WriteEntityList(buffer, obj.OrbStatusList);
            WriteEntityList(buffer, obj.MsgSetList);
            WriteEntityList(buffer, obj.ShortCutList);
            WriteEntityList(buffer, obj.CommunicationShortCutList);
            WriteEntity(buffer, obj.MatchingProfile);
            WriteEntity(buffer, obj.ArisenProfile);
            WriteBool(buffer, obj.HideEquipHead);
            WriteBool(buffer, obj.HideEquipLantern);
            WriteBool(buffer, obj.HideEquipHeadPawn);
            WriteBool(buffer, obj.HideEquipLanternPawn);
            WriteByte(buffer, obj.ArisenProfileShareRange);
            WriteByte(buffer, (byte) obj.OnlineStatus);
        }

        public override CDataCharacterInfo Read(IBuffer buffer)
        {
            CDataCharacterInfo obj = new CDataCharacterInfo();
            obj.CharacterId = ReadUInt32(buffer);
            obj.UserId = ReadUInt32(buffer);
            obj.Version = ReadUInt32(buffer);
            obj.FirstName = ReadMtString(buffer);
            obj.LastName = ReadMtString(buffer);
            obj.EditInfo = ReadEntity<CDataEditInfo>(buffer);
            obj.StatusInfo = ReadEntity<CDataStatusInfo>(buffer);
            obj.Job = (JobId) ReadByte(buffer);
            obj.CharacterJobDataList = ReadEntityList<CDataCharacterJobData>(buffer);
            obj.PlayPointList = ReadEntityList<CDataJobPlayPoint>(buffer);
            obj.CharacterEquipDataList = ReadEntityList<CDataCharacterEquipData>(buffer);
            obj.CharacterEquipViewDataList = ReadEntityList<CDataCharacterEquipData>(buffer);
            obj.CharacterEquipJobItemList = ReadEntityList<CDataEquipJobItem>(buffer);
            obj.JewelrySlotNum = ReadByte(buffer);
            obj.Unk0 = ReadEntityList<UnknownCharacterData0>(buffer);
            obj.CharacterItemSlotInfoList = ReadEntityList<CDataCharacterItemSlotInfo>(buffer);
            obj.WalletPointList = ReadEntityList<CDataWalletPoint>(buffer);
            obj.MyPawnSlotNum = ReadByte(buffer);
            obj.RentalPawnSlotNum = ReadByte(buffer);
            obj.OrbStatusList = ReadEntityList<CDataOrbPageStatus>(buffer);
            obj.MsgSetList = ReadEntityList<CDataCharacterMsgSet>(buffer);
            obj.ShortCutList = ReadEntityList<CDataShortCut>(buffer);
            obj.CommunicationShortCutList = ReadEntityList<CDataCommunicationShortCut>(buffer);
            obj.MatchingProfile = ReadEntity<CDataMatchingProfile>(buffer);
            obj.ArisenProfile = ReadEntity<CDataArisenProfile>(buffer);
            obj.HideEquipHead = ReadBool(buffer);
            obj.HideEquipLantern = ReadBool(buffer);
            obj.HideEquipHeadPawn = ReadBool(buffer);
            obj.HideEquipLanternPawn = ReadBool(buffer);
            obj.ArisenProfileShareRange = ReadByte(buffer);
            obj.OnlineStatus = (OnlineStatus) ReadByte(buffer);
            return obj;
        }
    }
}
