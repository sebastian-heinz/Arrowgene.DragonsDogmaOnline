using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataClanPartnerPawnInfo
    {
        public CDataClanPartnerPawnInfo()
        {
            MyPartnerPawnList = new();
            MemberPartnerPawnList = new();
        }

        public List<CDataCommonU32> MyPartnerPawnList { get; set; }
        public List<CDataCommonU32> MemberPartnerPawnList { get; set; }

        public class Serializer : EntitySerializer<CDataClanPartnerPawnInfo>
        {
            public override void Write(IBuffer buffer, CDataClanPartnerPawnInfo obj)
            {
                WriteEntityList<CDataCommonU32>(buffer, obj.MyPartnerPawnList);
                WriteEntityList<CDataCommonU32>(buffer, obj.MemberPartnerPawnList);
            }

            public override CDataClanPartnerPawnInfo Read(IBuffer buffer)
            {
                CDataClanPartnerPawnInfo obj = new CDataClanPartnerPawnInfo();
                obj.MyPartnerPawnList = ReadEntityList<CDataCommonU32>(buffer);
                obj.MemberPartnerPawnList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
