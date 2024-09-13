using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGachaDrawGroupInfo
    {
        public List<CDataGachaSettlementInfo> GachaSettlementList { get; set; }
        public List<CDataGachaDrawInfo> GachaDrawList { get; set; }

        public CDataGachaDrawGroupInfo()
        {
            GachaSettlementList = new List<CDataGachaSettlementInfo>();
            GachaDrawList = new List<CDataGachaDrawInfo>();
        }

        public class Serializer : EntitySerializer<CDataGachaDrawGroupInfo>
        {
            public override void Write(IBuffer buffer, CDataGachaDrawGroupInfo obj)
            {
                WriteEntityList<CDataGachaSettlementInfo>(buffer, obj.GachaSettlementList);
                WriteEntityList<CDataGachaDrawInfo>(buffer, obj.GachaDrawList);
            }

            public override CDataGachaDrawGroupInfo Read(IBuffer buffer)
            {
                CDataGachaDrawGroupInfo obj = new CDataGachaDrawGroupInfo
                {
                    GachaSettlementList = ReadEntityList<CDataGachaSettlementInfo>(buffer),
                    GachaDrawList = ReadEntityList<CDataGachaDrawInfo>(buffer)
                };

                return obj;
            }
        }
    }
}
