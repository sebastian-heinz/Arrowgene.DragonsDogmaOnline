using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMyMandragoraUnk1Unk7
    {
        public uint Unk0 { get; set; }
        public uint Unk1 { get; set; }
        public List<CDataMyMandragoraUnk1Unk7Unk2> Unk2 { get; set; } = new();
        public long Unk3 { get; set; }

        public class Serializer : EntitySerializer<CDataMyMandragoraUnk1Unk7>
        {
            public override void Write(IBuffer buffer, CDataMyMandragoraUnk1Unk7 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteEntityList<CDataMyMandragoraUnk1Unk7Unk2>(buffer, obj.Unk2);
                WriteInt64(buffer, obj.Unk3);
            }

            public override CDataMyMandragoraUnk1Unk7 Read(IBuffer buffer)
            {
                CDataMyMandragoraUnk1Unk7 obj = new CDataMyMandragoraUnk1Unk7();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2 = ReadEntityList<CDataMyMandragoraUnk1Unk7Unk2>(buffer);
                obj.Unk3 = ReadInt64(buffer);
                return obj;
            }
        }
    }
}
