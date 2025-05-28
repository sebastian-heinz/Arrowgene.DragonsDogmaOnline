using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCharacterInfo
    {
        public CDataCharacterInfo(Character character)
        {
            CharacterId = character.CharacterId;
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
                    Equips = character.Equipment.AsCDataEquipItemInfo(EquipType.Performance)
            }};
            CharacterEquipViewDataList = new List<CDataCharacterEquipData>() { new CDataCharacterEquipData() {
                    Equips = character.Equipment.AsCDataEquipItemInfo(EquipType.Visual)
            }};
            CharacterEquipJobItemList = character.EquipmentTemplate.JobItemsAsCDataEquipJobItem(character.Job);
            JewelrySlotNum = character.JewelrySlotNum;
            EmblemStatList = character.EmblemStatList;
            CharacterItemSlotInfoList = character.Storage.GetAllStoragesAsCDataCharacterItemSlotInfoList();
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
        }

        public uint CharacterId { get; set; }
        public uint UserId { get; set; }
        public uint Version { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public CDataEditInfo EditInfo { get; set; } = new();
        public CDataStatusInfo StatusInfo { get; set; } = new();
        public JobId Job { get; set; }
        public List<CDataCharacterJobData> CharacterJobDataList { get; set; } = new();
        public List<CDataJobPlayPoint> PlayPointList { get; set; } = new();
        public List<CDataCharacterEquipData> CharacterEquipDataList { get; set; } = new();
        public List<CDataCharacterEquipData> CharacterEquipViewDataList { get; set; } = new();
        public List<CDataEquipJobItem> CharacterEquipJobItemList { get; set; } = new();
        public byte JewelrySlotNum { get; set; }
        public List<CDataEquipStatParam> EmblemStatList { get; set; } = new();//from Ghidra
        public List<CDataCharacterItemSlotInfo> CharacterItemSlotInfoList { get; set; } = new();
        public List<CDataWalletPoint> WalletPointList { get; set; } = new();
        public byte MyPawnSlotNum { get; set; }
        public byte RentalPawnSlotNum { get; set; }
        public List<CDataOrbPageStatus> OrbStatusList { get; set; } = new();
        public List<CDataCharacterMsgSet> MsgSetList { get; set; } = new();
        public List<CDataShortCut> ShortCutList { get; set; } = new();
        public List<CDataCommunicationShortCut> CommunicationShortCutList { get; set; } = new();
        public CDataMatchingProfile MatchingProfile { get; set; } = new();
        public CDataArisenProfile ArisenProfile { get; set; } = new();
        public bool HideEquipHead { get; set; }
        public bool HideEquipLantern { get; set; }
        public bool HideEquipHeadPawn { get; set; }
        public bool HideEquipLanternPawn { get; set; }
        public byte ArisenProfileShareRange { get; set; }
        public OnlineStatus OnlineStatus { get; set; }

        public class Serializer : EntitySerializer<CDataCharacterInfo>
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
                WriteByte(buffer, (byte)obj.Job);
                WriteEntityList(buffer, obj.CharacterJobDataList);
                WriteEntityList(buffer, obj.PlayPointList);
                WriteEntityList(buffer, obj.CharacterEquipDataList);
                WriteEntityList(buffer, obj.CharacterEquipViewDataList);
                WriteEntityList(buffer, obj.CharacterEquipJobItemList);
                WriteByte(buffer, obj.JewelrySlotNum);
                WriteEntityList(buffer, obj.EmblemStatList);
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
                WriteByte(buffer, (byte)obj.OnlineStatus);
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
                obj.Job = (JobId)ReadByte(buffer);
                obj.CharacterJobDataList = ReadEntityList<CDataCharacterJobData>(buffer);
                obj.PlayPointList = ReadEntityList<CDataJobPlayPoint>(buffer);
                obj.CharacterEquipDataList = ReadEntityList<CDataCharacterEquipData>(buffer);
                obj.CharacterEquipViewDataList = ReadEntityList<CDataCharacterEquipData>(buffer);
                obj.CharacterEquipJobItemList = ReadEntityList<CDataEquipJobItem>(buffer);
                obj.JewelrySlotNum = ReadByte(buffer);
                obj.EmblemStatList = ReadEntityList<CDataEquipStatParam>(buffer);
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
                obj.OnlineStatus = (OnlineStatus)ReadByte(buffer);
                return obj;
            }
        }
    }
}
