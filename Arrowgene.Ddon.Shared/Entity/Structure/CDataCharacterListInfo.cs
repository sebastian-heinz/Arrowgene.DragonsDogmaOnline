using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCharacterListInfo
    {
        public CDataCharacterListInfo()
        {
            CharacterListElement = new CDataCharacterListElement();
            EditInfo = new CDataEditInfo();
            MatchingProfile = new CDataMatchingProfile();
            EquipItemInfo = new List<CDataEquipItemInfo>();
            GpCourseValidList = new List<CDataGPCourseValid>();
            ClanName = "";
            ClanNameShort = "";
            IsClanMemberNotice = 0;
        }

        public CDataCharacterListElement CharacterListElement { get; set; }
        public CDataEditInfo EditInfo { get; set; }
        public CDataMatchingProfile MatchingProfile { get; set; }
        public List<CDataEquipItemInfo> EquipItemInfo { get; set; }
        public List<CDataGPCourseValid> GpCourseValidList { get; set; }
        public byte NextFlowType { get; set; }
        public string ClanName { get; set; }
        public string ClanNameShort { get; set; }
        public byte IsClanMemberNotice { get; set; }

        public class Serializer : EntitySerializer<CDataCharacterListInfo>
        {
            public override void Write(IBuffer buffer, CDataCharacterListInfo obj)
            {
                WriteEntity(buffer, obj.CharacterListElement);
                WriteEntity(buffer, obj.EditInfo);
                WriteEntity(buffer, obj.MatchingProfile);
                WriteEntityList(buffer, obj.EquipItemInfo);
                WriteEntityList(buffer, obj.GpCourseValidList);
                WriteByte(buffer, obj.NextFlowType);
                WriteMtString(buffer, obj.ClanName);
                WriteMtString(buffer, obj.ClanNameShort);
                WriteByte(buffer, obj.IsClanMemberNotice);
            }

            public override CDataCharacterListInfo Read(IBuffer buffer)
            {
                CDataCharacterListInfo obj = new CDataCharacterListInfo();
                obj.CharacterListElement = ReadEntity<CDataCharacterListElement>(buffer);
                obj.EditInfo = ReadEntity<CDataEditInfo>(buffer);
                obj.MatchingProfile = ReadEntity<CDataMatchingProfile>(buffer);
                obj.EquipItemInfo = ReadEntityList<CDataEquipItemInfo>(buffer);
                obj.GpCourseValidList = ReadEntityList<CDataGPCourseValid>(buffer);
                obj.NextFlowType = ReadByte(buffer);
                obj.ClanName = ReadMtString(buffer);
                obj.ClanNameShort = ReadMtString(buffer);
                obj.IsClanMemberNotice = ReadByte(buffer);
                return obj;
            }
        }
    }
}
