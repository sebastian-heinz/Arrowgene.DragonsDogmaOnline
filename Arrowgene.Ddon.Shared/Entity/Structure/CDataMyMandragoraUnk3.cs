using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMyMandragoraUnk3
    {
        public byte Unk0 { get; set; }
        public uint Unk1 { get; set; }

        public class Serializer : EntitySerializer<CDataMyMandragoraUnk3>
        {
            public override void Write(IBuffer buffer, CDataMyMandragoraUnk3 obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
            }

            public override CDataMyMandragoraUnk3 Read(IBuffer buffer)
            {
                CDataMyMandragoraUnk3 obj = new CDataMyMandragoraUnk3();
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
