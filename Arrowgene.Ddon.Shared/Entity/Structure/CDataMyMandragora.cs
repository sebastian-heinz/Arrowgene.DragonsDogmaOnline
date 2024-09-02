using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMyMandragora
    {
        public uint Unk0 { get; set; }
        public byte Unk1 { get; set; }
        public uint MandragoraId { get; set; }
        public string MandragoraName { get; set; }
        public uint Unk4 { get; set; }
        public long Unk5 { get; set; }
        public uint Unk6 { get; set; }
        public CDataMyMandragoraUnk1Unk7 Unk7 { get; set; }

        public class Serializer : EntitySerializer<CDataMyMandragora>
        {
            public override void Write(IBuffer buffer, CDataMyMandragora obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.MandragoraId);
                WriteMtString(buffer, obj.MandragoraName);
                WriteUInt32(buffer, obj.Unk4);
                WriteInt64(buffer, obj.Unk5);
                WriteUInt32(buffer, obj.Unk6);
                WriteEntity<CDataMyMandragoraUnk1Unk7>(buffer, obj.Unk7);
            }

            public override CDataMyMandragora Read(IBuffer buffer)
            {
                CDataMyMandragora obj = new CDataMyMandragora();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadByte(buffer);
                obj.MandragoraId = ReadUInt32(buffer);
                obj.MandragoraName = ReadMtString(buffer);
                obj.Unk4 = ReadUInt32(buffer);
                obj.Unk5 = ReadInt64(buffer);
                obj.Unk6 = ReadUInt32(buffer);
                obj.Unk7 = ReadEntity<CDataMyMandragoraUnk1Unk7>(buffer);
                return obj;
            }
        }
    }
}
