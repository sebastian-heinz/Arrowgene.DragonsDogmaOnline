using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMyMandragoraBeginCraftResUnk0
    {
        public uint SpeciesIndex { get; set; }
        public byte Unk1 { get; set; }
        public uint MandragoraId { get; set; }
        public string Unk3 { get; set; } = string.Empty;
        public uint Unk4 { get; set; }
        public long Unk5 { get; set; }
        public uint Unk6 { get; set; }
        public CDataMyMandragoraBeginCraftResUnk0Unk7 Unk7 { get; set; } = new();

        public class Serializer : EntitySerializer<CDataMyMandragoraBeginCraftResUnk0>
        {
            public override void Write(IBuffer buffer, CDataMyMandragoraBeginCraftResUnk0 obj)
            {
                WriteUInt32(buffer, obj.SpeciesIndex);
                WriteByte(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.MandragoraId);
                WriteMtString(buffer, obj.Unk3);
                WriteUInt32(buffer, obj.Unk4);
                WriteInt64(buffer, obj.Unk5);
                WriteUInt32(buffer, obj.Unk6);
                WriteEntity<CDataMyMandragoraBeginCraftResUnk0Unk7>(buffer, obj.Unk7);
            }

            public override CDataMyMandragoraBeginCraftResUnk0 Read(IBuffer buffer)
            {
                CDataMyMandragoraBeginCraftResUnk0 obj = new CDataMyMandragoraBeginCraftResUnk0();
                obj.SpeciesIndex = ReadUInt32(buffer);
                obj.Unk1 = ReadByte(buffer);
                obj.MandragoraId = ReadUInt32(buffer);
                obj.Unk3 = ReadMtString(buffer);
                obj.Unk4 = ReadUInt32(buffer);
                obj.Unk5 = ReadInt64(buffer);
                obj.Unk6 = ReadUInt32(buffer);
                obj.Unk7 = ReadEntity<CDataMyMandragoraBeginCraftResUnk0Unk7>(buffer);
                return obj;
            }
        }
    }
}
