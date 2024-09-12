using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMyMandragoraUnk1Unk7Unk2
    {
        public uint Unk0 { get; set; }
        public ushort Unk1 { get; set; }

        public class Serializer : EntitySerializer<CDataMyMandragoraUnk1Unk7Unk2>
        {
            public override void Write(IBuffer buffer, CDataMyMandragoraUnk1Unk7Unk2 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt16(buffer, obj.Unk1);
            }

            public override CDataMyMandragoraUnk1Unk7Unk2 Read(IBuffer buffer)
            {
                CDataMyMandragoraUnk1Unk7Unk2 obj = new CDataMyMandragoraUnk1Unk7Unk2();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
