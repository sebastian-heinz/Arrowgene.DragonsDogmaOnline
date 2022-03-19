using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Client.Resource
{
    public class LandListLal : ResourceFile
    {
        public class LandInfo
        {
            public uint LandId { get; set; }
            public bool IsDispNews { get; set; }
            public byte GameMode { get; set; }
            public List<uint> AreaIds { get; set; }

            public LandInfo()
            {
                AreaIds = new List<uint>();
            }
        }

        public List<LandInfo> LandInfos { get; }

        public LandListLal()
        {
            LandInfos = new List<LandInfo>();
        }

        protected override void ReadResource(IBuffer buffer)
        {
            uint version = ReadUInt32(buffer);
            LandInfos.Clear();
            List<LandInfo> infos = ReadMtArray(buffer, ReadEntry);
            LandInfos.AddRange(infos);
        }

        private LandInfo ReadEntry(IBuffer buffer)
        {
            LandInfo entry = new LandInfo();
            entry.LandId = ReadUInt32(buffer);
            entry.IsDispNews = ReadBool(buffer);
            entry.GameMode = ReadByte(buffer);
            entry.AreaIds = ReadMtArray(buffer, ReadAreaId);
            return entry;
        }

        private uint ReadAreaId(IBuffer buffer)
        {
            return ReadUInt32(buffer);
        }
    }
}
