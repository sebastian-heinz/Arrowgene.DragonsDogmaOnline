using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public struct CDataCharacterListInfo
    {
        public CDataCharacterListElement Element;
        public CDataEditInfo EditInfo;

        public CDataMatchingProfile MatchingProfile;

        // size prefix
        public List<CDataEquipItemInfo> EquipItemInfo;

        // size prefix
        public List<CDataGPCourseValid> GpCourseValidList;

        public byte NextFlowType;

        // length prefix
        public string ClanName;

        // length prefix
        public string ClanNameShort;
        public byte IsClanMemberNotice;
    }

    public class CDataCharacterListInfoSerializer : EntitySerializer<CDataCharacterListInfo>
    {
        public override void Write(IBuffer buffer, CDataCharacterListInfo obj)
        {
            WriteEntity(buffer, obj.Element);
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
            obj.Element = ReadEntity<CDataCharacterListElement>(buffer);
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
