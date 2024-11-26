using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSeasonDungeonUnk0
    {
        public CDataSeasonDungeonUnk0()
        {
        }

        public uint BuffId { get; set; }
        public uint Level { get; set; }

        public class Serializer : EntitySerializer<CDataSeasonDungeonUnk0>
        {
            public override void Write(IBuffer buffer, CDataSeasonDungeonUnk0 obj)
            {
                WriteUInt32(buffer, obj.BuffId);
                WriteUInt32(buffer, obj.Level);
            }

            public override CDataSeasonDungeonUnk0 Read(IBuffer buffer)
            {
                CDataSeasonDungeonUnk0 obj = new CDataSeasonDungeonUnk0();
                obj.BuffId = ReadUInt32(buffer);
                obj.Level = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

