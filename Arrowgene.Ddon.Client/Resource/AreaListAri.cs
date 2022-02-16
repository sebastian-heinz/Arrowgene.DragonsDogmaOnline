using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Client.Resource
{
    public class AreaListAri : ResourceFile
    {
        public class AreaInfo
        {
            public uint AreaId { get; set; }
            public uint PosX { get; set; }
            public uint PosY { get; set; }
        }

        public List<AreaInfo> AreaInfos { get; }

        public AreaListAri()
        {
            AreaInfos = new List<AreaInfo>();
        }

        protected override void ReadResource(IBuffer buffer)
        {
            AreaInfos.Clear();
            AreaInfos.AddRange(ReadMtArray(buffer, ReadEntry));
        }

        private AreaInfo ReadEntry(IBuffer buffer)
        {
            AreaInfo ai = new AreaInfo();
            ai.AreaId = ReadUInt32(buffer);
            ai.PosX = ReadUInt32(buffer);
            ai.PosY = ReadUInt32(buffer);
            return ai;
        }
    }
}
