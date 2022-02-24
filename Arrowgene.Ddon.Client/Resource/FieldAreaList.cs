using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Client.Resource
{
    /// <summary>
    /// StageNo = signed int 32
    /// </summary>
    public class FieldAreaList : ResourceFile
    {
        public class FieldAreaInfo
        {
            public List<int> StageNoList { get; set; }
            public List<int> BelongStageNoList { get; set; }
            public uint FieldAreaId { get; set; }
            public uint GmdIdx { get; set; }
            public uint VersionId { get; set; }
            public ushort LandId  { get; set; }
            public ushort AreaId { get; set; }
            
            public FieldAreaInfo()
            {
                StageNoList = new List<int>();
                BelongStageNoList = new List<int>();
            }
        }

        public List<FieldAreaInfo> FieldAreaInfos { get; }

        public FieldAreaList()
        {
            FieldAreaInfos = new List<FieldAreaInfo>();
        }

        protected override MagicIdWidth IdWidth => MagicIdWidth.UInt;
        
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

        private int ReadStageNo(IBuffer buffer)
        {
            return ReadInt32(buffer);
        }
    }
}
