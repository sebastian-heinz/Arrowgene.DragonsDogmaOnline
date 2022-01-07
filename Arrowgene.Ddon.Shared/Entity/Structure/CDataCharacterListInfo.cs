using System;
using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public struct CDataCharacterListInfo
    {
        CDataCharacterListElement Element;
        CDataEditInfo EditInfo;
        CDataMatchingProfile MatchingProfile; 
        // size prefix
        List<CDataEquipItemInfo> EquipItemInfo;
        // size prefix
        List<CDataGPCourseValid> GpCourseValidList;
        byte NextFlowType;
        // length prefix
        string ClanName;
        // length prefix
        string ClanNameShort;
        byte IsClanMemberNotice;
    }

    public class CDataCharacterListInfoSerializer : EntitySerializer<CDataCharacterListInfo>
    {
        public override void Write(IBuffer buffer, CDataCharacterListInfo obj)
        {
            throw new NotImplementedException();
        }

        public override CDataCharacterListInfo Read(IBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
