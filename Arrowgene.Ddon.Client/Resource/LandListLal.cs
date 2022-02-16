using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Client.Resource
{
    public class LandListLal : ResourceFile
    {
        public class Info
        {
            public uint LandId { get; set; }
            public ushort Unk { get; set; }
            public List<uint> AreaIds { get; set; }

            public Info()
            {
                AreaIds = new List<uint>();
            }
        }

        public List<Info> Infos { get; }

        public LandListLal()
        {
            Infos = new List<Info>();
        }

        protected override void ReadResource(IBuffer buffer)
        {
            Infos.Clear();
            List<Info> infos = ReadMtArray(buffer, ReadEntry);
            Infos.AddRange(infos);
        }

        private Info ReadEntry(IBuffer buffer)
        {
            Info entry = new Info();
            entry.LandId = ReadUInt32(buffer);
            entry.Unk = ReadUInt16(buffer);
            entry.AreaIds = ReadMtArray(buffer, ReadAreaId);
            return entry;
        }

        private uint ReadAreaId(IBuffer buffer)
        {
            return ReadUInt32(buffer);
        }
    }
}
