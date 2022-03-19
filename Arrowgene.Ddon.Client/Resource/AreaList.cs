using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Client.Resource
{
    public class AreaList : ResourceFile
    {
        public class AreaInfo
        {
            public uint AreaId { get; set; }
            public int PosX { get; set; }
            public int PosY { get; set; }
        }

        public List<AreaInfo> AreaInfos { get; }

        public AreaList()
        {
            AreaInfos = new List<AreaInfo>();
        }

        protected override void ReadResource(IBuffer buffer)
        {
            uint version = ReadUInt32(buffer);
            AreaInfos.Clear();
            AreaInfos.AddRange(ReadMtArray(buffer, ReadEntry));
        }

        private AreaInfo ReadEntry(IBuffer buffer)
        {
            AreaInfo ai = new AreaInfo();
            ai.AreaId = ReadUInt32(buffer);
            ai.PosX = ReadInt32(buffer);
            ai.PosY = ReadInt32(buffer);
            return ai;
        }
    }
}
