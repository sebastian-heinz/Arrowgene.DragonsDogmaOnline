using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataClanConciergeInfo
    {
        public CDataClanConciergeInfo()
        {
            ClanConciergeNpcList = new();
        }

        public uint NpcId { get; set; }
        public List<CDataClanConciergeNpc> ClanConciergeNpcList { get; set; }


        public class Serializer : EntitySerializer<CDataClanConciergeInfo>
        {
            public override void Write(IBuffer buffer, CDataClanConciergeInfo obj)
            {
                WriteUInt32(buffer, obj.NpcId);
                WriteEntityList<CDataClanConciergeNpc>(buffer, obj.ClanConciergeNpcList);
            }

            public override CDataClanConciergeInfo Read(IBuffer buffer)
            {
                CDataClanConciergeInfo obj = new CDataClanConciergeInfo();
                obj.NpcId = ReadUInt32(buffer);
                obj.ClanConciergeNpcList = ReadEntityList<CDataClanConciergeNpc>(buffer);
                return obj;
            }
        }
    }
}
