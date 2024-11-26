using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSeasonDungeonUnk2
    {
        public CDataSeasonDungeonUnk2()
        {
            Label = string.Empty;
        }

        public uint BuffId { get; set; } // ID 10
        public string Label { get; set; }
        public uint Unk2 { get; set; }
        public uint Unk3 { get; set; }
        public uint Unk4 { get; set; }
        public uint Unk5 { get; set; }
        public uint Unk6 { get; set; }


        public class Serializer : EntitySerializer<CDataSeasonDungeonUnk2>
        {
            public override void Write(IBuffer buffer, CDataSeasonDungeonUnk2 obj)
            {
                WriteUInt32(buffer, obj.BuffId);
                WriteMtString(buffer, obj.Label);
                WriteUInt32(buffer, obj.Unk2);
                WriteUInt32(buffer, obj.Unk3);
                WriteUInt32(buffer, obj.Unk4);
                WriteUInt32(buffer, obj.Unk5);
                WriteUInt32(buffer, obj.Unk6);
            }

            public override CDataSeasonDungeonUnk2 Read(IBuffer buffer)
            {
                CDataSeasonDungeonUnk2 obj = new CDataSeasonDungeonUnk2();
                obj.BuffId = ReadUInt32(buffer);
                obj.Label = ReadMtString(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                obj.Unk3 = ReadUInt32(buffer);
                obj.Unk4 = ReadUInt32(buffer);
                obj.Unk5 = ReadUInt32(buffer);
                obj.Unk6 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}


