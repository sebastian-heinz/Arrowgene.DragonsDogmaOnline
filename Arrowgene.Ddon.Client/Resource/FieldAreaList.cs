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
            public ushort LandId { get; set; }
            public ushort AreaId { get; set; }

            public FieldAreaInfo()
            {
                StageNoList = new List<int>();
                BelongStageNoList = new List<int>();
            }
        }

        public static FieldAreaInfo Get(List<FieldAreaInfo> fieldAreaInfos, ushort areaId, ushort landId, int stageNo)
        {
            foreach (FieldAreaInfo fai in fieldAreaInfos)
            {
                if (fai.AreaId != 0 && fai.AreaId != areaId)
                {
                    // filter on AreaId only if it is not zero
                    continue;
                }

                if (fai.LandId == landId && fai.StageNoList.Contains(stageNo))
                {
                    return fai;
                }
            }

            return null;
        }

        public List<FieldAreaInfo> FieldAreaInfos { get; }

        public FieldAreaList()
        {
            FieldAreaInfos = new List<FieldAreaInfo>();
        }

        protected override void ReadResource(IBuffer buffer)
        {
            uint version = ReadUInt32(buffer);
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
