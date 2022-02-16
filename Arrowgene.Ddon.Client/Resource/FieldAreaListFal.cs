using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Client.Resource
{
    public class FieldAreaListFal : ClientResourceFile
    {
        public class FieldAreaInfo
        {
            public List<uint> StageNoList { get; set; }
            public List<uint> BelongStageNoList { get; set; }
            public uint FieldAreaId { get; set; }
            public uint GmdIdx { get; set; }
            public uint VersionId { get; set; }
            public uint LandId  { get; set; }
            public uint AreaId { get; set; }
        }

        public List<FieldAreaInfo> FieldAreaInfos { get; }

        public FieldAreaListFal()
        {
            FieldAreaInfos = new List<FieldAreaInfo>();
        }

        protected override void Read(IBuffer buffer)
        {
            FieldAreaInfos.Clear();
            List<FieldAreaInfo> infos = ReadMtArray(buffer, ReadEntry);
            FieldAreaInfos.AddRange(infos);
        }

        private FieldAreaInfo ReadEntry(IBuffer buffer)
        {
            FieldAreaInfo entry = new FieldAreaInfo();
            // TODO verify - Structure different from debug symbols
            entry.FieldAreaId = ReadUInt32(buffer); 
            entry.AreaId = ReadUInt32(buffer);
            entry.LandId = ReadUInt32(buffer);
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
