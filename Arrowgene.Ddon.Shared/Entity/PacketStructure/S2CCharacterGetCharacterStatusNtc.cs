using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterGetCharacterStatusNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CHARACTER_GET_CHARACTER_STATUS_NTC;

        public S2CCharacterGetCharacterStatusNtc()
        {
            StatusInfo = new CDataStatusInfo();
            JobParam = new CDataCharacterJobData();
            CharacterParam = new CDataCharacterLevelParam();
            EditInfo = new CDataEditInfo();
            EquipDataList = new List<CDataEquipItemInfo>();
            VisualEquipDataList = new List<CDataEquipItemInfo>();
            EquipJobItemList = new List<CDataEquipJobItem>();
            EquipElementParams = new List<CDataEquipElementParam>();
        }

        public uint CharacterId { get; set; }
        public CDataStatusInfo StatusInfo { get; set; }
        public CDataCharacterJobData JobParam { get; set; }
        public CDataCharacterLevelParam CharacterParam { get; set; }
        public CDataEditInfo EditInfo { get; set; }
        public List<CDataEquipItemInfo> EquipDataList { get; set; }
        public List<CDataEquipItemInfo> VisualEquipDataList { get; set; }
        public List<CDataEquipJobItem> EquipJobItemList { get; set; }
        public List<CDataEquipElementParam> EquipElementParams { get; set; }
        public bool HideHead { get; set; }
        public bool HideLantern { get; set; }
        public uint JewelryNum { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCharacterGetCharacterStatusNtc>
        {
            public override void Write(IBuffer buffer, S2CCharacterGetCharacterStatusNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteEntity<CDataStatusInfo>(buffer, obj.StatusInfo);
                WriteEntity<CDataCharacterJobData>(buffer, obj.JobParam);
                WriteEntity<CDataCharacterLevelParam>(buffer, obj.CharacterParam);
                WriteEntity<CDataEditInfo>(buffer, obj.EditInfo);
                WriteEntityList<CDataEquipItemInfo>(buffer, obj.EquipDataList);
                WriteEntityList<CDataEquipItemInfo>(buffer, obj.VisualEquipDataList);
                WriteEntityList<CDataEquipJobItem>(buffer, obj.EquipJobItemList);
                WriteEntityList<CDataEquipElementParam>(buffer, obj.EquipElementParams);
                WriteBool(buffer, obj.HideHead);
                WriteBool(buffer, obj.HideLantern);
                WriteUInt32(buffer, obj.JewelryNum);
            }

            public override S2CCharacterGetCharacterStatusNtc Read(IBuffer buffer)
            {
                S2CCharacterGetCharacterStatusNtc obj = new S2CCharacterGetCharacterStatusNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.StatusInfo = ReadEntity<CDataStatusInfo>(buffer);
                obj.JobParam = ReadEntity<CDataCharacterJobData>(buffer);
                obj.CharacterParam = ReadEntity<CDataCharacterLevelParam>(buffer);
                obj.EditInfo = ReadEntity<CDataEditInfo>(buffer);
                obj.EquipDataList = ReadEntityList<CDataEquipItemInfo>(buffer);
                obj.VisualEquipDataList = ReadEntityList<CDataEquipItemInfo>(buffer);
                obj.EquipJobItemList = ReadEntityList<CDataEquipJobItem>(buffer);
                obj.EquipElementParams = ReadEntityList<CDataEquipElementParam>(buffer);
                obj.HideHead = ReadBool(buffer);
                obj.HideLantern = ReadBool(buffer);
                obj.JewelryNum = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
