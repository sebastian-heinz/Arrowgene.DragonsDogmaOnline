using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSoulOrdealUnk1
    {
        public CDataSoulOrdealUnk1()
        {
        }

        public uint ItemId { get; set; }
        public uint Unk1 { get; set; }
        public uint Unk2 { get; set; }
        public byte Unk3 { get; set; }
        public byte Unk4 { get; set; }
        public bool Unk5 { get; set; }
        public bool Unk6 { get; set; }
        public bool Unk7 { get; set; }

        public class Serializer : EntitySerializer<CDataSoulOrdealUnk1>
        {
            public override void Write(IBuffer buffer, CDataSoulOrdealUnk1 obj)
            {
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt32(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Unk2);
                WriteByte(buffer, obj.Unk3);
                WriteByte(buffer, obj.Unk4);
                WriteBool(buffer, obj.Unk5);
                WriteBool(buffer, obj.Unk6);
                WriteBool(buffer, obj.Unk7);
            }

            public override CDataSoulOrdealUnk1 Read(IBuffer buffer)
            {
                CDataSoulOrdealUnk1 obj = new CDataSoulOrdealUnk1();
                obj.ItemId = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                obj.Unk3 = ReadByte(buffer);
                obj.Unk4 = ReadByte(buffer);
                obj.Unk5 = ReadBool(buffer);
                obj.Unk6 = ReadBool(buffer);
                obj.Unk7 = ReadBool(buffer);
                return obj;
            }
        }
    }
}
