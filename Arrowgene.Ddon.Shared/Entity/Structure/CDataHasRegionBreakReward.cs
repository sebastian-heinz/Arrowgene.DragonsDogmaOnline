using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataHasRegionBreakReward
    {
        public CDataHasRegionBreakReward()
        {
        }

        public byte RegionNo {  get; set; }

        public class Serializer : EntitySerializer<CDataHasRegionBreakReward>
        {
            public override void Write(IBuffer buffer, CDataHasRegionBreakReward obj)
            {
                WriteByte(buffer, obj.RegionNo);
            }

            public override CDataHasRegionBreakReward Read(IBuffer buffer)
            {
                CDataHasRegionBreakReward obj = new CDataHasRegionBreakReward();
                obj.RegionNo = ReadByte(buffer);
                return obj;
            }
        }
    }
}
