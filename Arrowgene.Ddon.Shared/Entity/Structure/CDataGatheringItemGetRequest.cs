using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGatheringItemGetRequest
    {
        public uint SlotNo { get; set; }
        public uint Num { get; set; }

        public class Serializer : EntitySerializer<CDataGatheringItemGetRequest>
        {
            public override void Write(IBuffer buffer, CDataGatheringItemGetRequest obj)
            {
                WriteUInt32(buffer, obj.SlotNo);
                WriteUInt32(buffer, obj.Num);
            }

            public override CDataGatheringItemGetRequest Read(IBuffer buffer)
            {
                CDataGatheringItemGetRequest obj = new CDataGatheringItemGetRequest();
                obj.SlotNo = ReadUInt32(buffer);
                obj.Num = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}