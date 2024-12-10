using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSeasonDungeonInfo
    {
        public CDataSeasonDungeonInfo()
        {
            Name = string.Empty;
        }

        public uint Unk0 { get; set; }
        public string Name { get; set; }
        public uint DungeonId { get; set; }

        public class Serializer : EntitySerializer<CDataSeasonDungeonInfo>
        {
            public override void Write(IBuffer buffer, CDataSeasonDungeonInfo obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteMtString(buffer, obj.Name);
                WriteUInt32(buffer, obj.DungeonId);
            }

            public override CDataSeasonDungeonInfo Read(IBuffer buffer)
            {
                CDataSeasonDungeonInfo obj = new CDataSeasonDungeonInfo();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Name = ReadMtString(buffer);
                obj.DungeonId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

