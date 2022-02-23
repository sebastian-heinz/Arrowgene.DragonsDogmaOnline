using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Client.Resource
{
    public class FieldAreaList : ResourceFile
    {
        //  rFieldAreaList::cFieldAreaInfo vftable:0x1C5F6D0, Size:0x3C, CRC32:0x2B8194
        public class FieldAreaInfo
        {
            public List<uint> StageNoList { get; set; }
            public List<uint> BelongStageNoList { get; set; }
            public uint FieldAreaId { get; set; }
            public uint GmdIdx { get; set; }
            public uint VersionId { get; set; }
            public ushort LandId  { get; set; }
            public ushort AreaId { get; set; }
            
            public FieldAreaInfo()
            {
                StageNoList = new List<uint>();
                BelongStageNoList = new List<uint>();
            }
        }

        public List<FieldAreaInfo> FieldAreaInfos { get; }

        public FieldAreaList()
        {
            FieldAreaInfos = new List<FieldAreaInfo>();
        }

        protected override void ReadResource(IBuffer buffer)
        {
            FieldAreaInfos.Clear();
            List<FieldAreaInfo> infos = ReadMtArray(buffer, ReadEntry);
            FieldAreaInfos.AddRange(infos);
        }
        
        private FieldAreaInfo ReadEntry(IBuffer buffer)
        {
            FieldAreaInfo entry = new FieldAreaInfo();
            entry.FieldAreaId = ReadUInt32(buffer); 
            entry.GmdIdx = ReadUInt32(buffer);
            entry.VersionId = 0x00009900;
            entry.LandId = ReadUInt16(buffer);
            entry.AreaId = ReadUInt16(buffer);
            entry.StageNoList = ReadMtArray(buffer, ReadStageNo);
            entry.BelongStageNoList = ReadMtArray(buffer, ReadStageNo);
            return entry;
        }

        private uint ReadStageNo(IBuffer buffer)
        {
            return ReadUInt32(buffer);
        }
    }
}
