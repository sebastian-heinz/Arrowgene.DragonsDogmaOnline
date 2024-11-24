using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSeasonDungeonSection
    {
        public CDataSeasonDungeonSection()
        {
            Unk4 = new CDataSeasonDungeonUnk0();
            SectionName = string.Empty;
        }

        public uint Unk0 { get; set; }
        public uint Unk1 { get; set; }
        public uint Unk2 { get; set; }
        public uint Unk3 { get; set; }
        public CDataSeasonDungeonUnk0 Unk4 {get; set;}
        public bool Unk5 { get; set; }
        public string SectionName { get; set; }

        public class Serializer : EntitySerializer<CDataSeasonDungeonSection>
        {
            public override void Write(IBuffer buffer, CDataSeasonDungeonSection obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Unk2);
                WriteUInt32(buffer, obj.Unk3);
                WriteEntity(buffer, obj.Unk4);
                WriteBool(buffer, obj.Unk5);
                WriteMtString(buffer, obj.SectionName);
            }

            public override CDataSeasonDungeonSection Read(IBuffer buffer)
            {
                CDataSeasonDungeonSection obj = new CDataSeasonDungeonSection();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                obj.Unk3 = ReadUInt32(buffer);
                obj.Unk4 = ReadEntity<CDataSeasonDungeonUnk0>(buffer);
                obj.Unk5 = ReadBool(buffer);
                obj.SectionName = ReadMtString(buffer);
                return obj;
            }
        }
    }
}

