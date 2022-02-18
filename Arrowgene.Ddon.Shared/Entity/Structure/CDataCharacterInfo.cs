using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCharacterInfo
    {
        public CDataCharacterInfo()
        {
            CharacterID = 0;
            UserID = 0;
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
            CharacterItemSlotInfoList = new List<CDataEquipElementParam>();
            UnkCharData0 = new List<UnknownCharacterData0>();
            UnkCharData1 = new List<UnknownCharacterData1>();
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

        public uint CharacterID;
        public uint UserID;
        public uint Version;
        public string FirstName;
        public string LastName;
        public CDataEditInfo EditInfo;
        public CDataStatusInfo StatusInfo;
        public byte Job;
        public List<CDataCharacterJobData> CharacterJobDataList;
        public List<CDataJobPlayPoint> PlayPointList;
        public List<CDataCharacterEquipData> CharacterEquipDataList;
        public List<CDataCharacterEquipData> CharacterEquipViewDataList;
        public List<CDataEquipJobItem> CharacterEquipJobItemList;
        public byte JewelrySlotNum;
        public List<CDataEquipElementParam> CharacterItemSlotInfoList;

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
        public byte OnlineStatus;
    }
    public class CDataCharacterInfoSerializer : EntitySerializer<CDataCharacterInfo>
    {
        public override void Write(IBuffer buffer, CDataCharacterInfo obj)
        {
            WriteUInt32(buffer, obj.CharacterID);
            WriteUInt32(buffer, obj.UserID);
            WriteUInt32(buffer, obj.Version);
            WriteMtString(buffer, obj.FirstName);
            WriteMtString(buffer, obj.LastName);
            WriteEntity(buffer, obj.EditInfo);
            WriteEntity(buffer, obj.StatusInfo);
            WriteByte(buffer, obj.Job);
            WriteEntityList(buffer, obj.CharacterJobDataList);
            WriteEntityList(buffer, obj.PlayPointList);
            WriteEntityList(buffer, obj.CharacterEquipDataList);
            WriteEntityList(buffer, obj.CharacterEquipViewDataList);
            WriteEntityList(buffer, obj.CharacterEquipJobItemList);
            WriteByte(buffer, obj.JewelrySlotNum);
            WriteEntityList(buffer, obj.CharacterItemSlotInfoList);
            WriteEntityList(buffer, obj.UnkCharData0);
            WriteEntityList(buffer, obj.UnkCharData1);
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
            WriteByte(buffer, obj.OnlineStatus);
        }

        public override CDataCharacterInfo Read(IBuffer buffer)
        {
            CDataCharacterInfo obj = new CDataCharacterInfo();
            obj.CharacterID = ReadUInt32(buffer);
            obj.UserID = ReadUInt32(buffer);
            obj.Version = ReadUInt32(buffer);
            obj.FirstName = ReadMtString(buffer);
            obj.LastName = ReadMtString(buffer);
            obj.EditInfo = ReadEntity<CDataEditInfo>(buffer);
            obj.StatusInfo = ReadEntity<CDataStatusInfo>(buffer);
            obj.Job = ReadByte(buffer);
            obj.CharacterJobDataList = ReadEntityList<CDataCharacterJobData>(buffer);
            obj.PlayPointList = ReadEntityList<CDataJobPlayPoint>(buffer);
            obj.CharacterEquipDataList = ReadEntityList<CDataCharacterEquipData>(buffer);
            obj.CharacterEquipViewDataList = ReadEntityList<CDataCharacterEquipData>(buffer);
            obj.CharacterEquipJobItemList = ReadEntityList<CDataEquipJobItem>(buffer);
            obj.JewelrySlotNum = ReadByte(buffer);
            obj.CharacterItemSlotInfoList = ReadEntityList<CDataEquipElementParam>(buffer);
            obj.UnkCharData0 = ReadEntityList<UnknownCharacterData0>(buffer);
            obj.UnkCharData1 = ReadEntityList<UnknownCharacterData1>(buffer);
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
            obj.OnlineStatus = ReadByte(buffer);
            return obj;
        }
    }
}
