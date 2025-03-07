using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMyMandragoraBeginCraftResUnk0Unk7
    {
        public uint Unk0 { get; set; }
        public uint Unk1 { get; set; }
        public List<CDataMyMandragoraBeginCraftResUnk0Unk7Unk2> Unk2 { get; set; }
        public long Unk3 { get; set; }

        public CDataMyMandragoraBeginCraftResUnk0Unk7()
        {
            Unk2 = new List<CDataMyMandragoraBeginCraftResUnk0Unk7Unk2>();
        }

        public class Serializer : EntitySerializer<CDataMyMandragoraBeginCraftResUnk0Unk7>
        {
            public override void Write(IBuffer buffer, CDataMyMandragoraBeginCraftResUnk0Unk7 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteEntityList<CDataMyMandragoraBeginCraftResUnk0Unk7Unk2>(buffer, obj.Unk2);
                WriteInt64(buffer, obj.Unk3);
            }

            public override CDataMyMandragoraBeginCraftResUnk0Unk7 Read(IBuffer buffer)
            {
                CDataMyMandragoraBeginCraftResUnk0Unk7 obj = new CDataMyMandragoraBeginCraftResUnk0Unk7();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2 = ReadEntityList<CDataMyMandragoraBeginCraftResUnk0Unk7Unk2>(buffer);
                obj.Unk3 = ReadInt64(buffer);
                return obj;
            }
        }
    }
}
