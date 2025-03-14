using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2C_INSTANCE_13_36_16_NTC : IPacketStructure
    {
        public S2C_INSTANCE_13_36_16_NTC()
        {
            Unk1List = new List<CDataCommonPair<uint>>();
        }

        public PacketId Id => PacketId.S2C_INSTANCE_13_36_16_NTC;

        public uint Unk0 { get; set; }
        public List<CDataCommonPair<uint>> Unk1List { get; set; }

        public class Serializer : PacketEntitySerializer<S2C_INSTANCE_13_36_16_NTC>
        {
            public override void Write(IBuffer buffer, S2C_INSTANCE_13_36_16_NTC obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteEntityList(buffer, obj.Unk1List);
            }

            public override S2C_INSTANCE_13_36_16_NTC Read(IBuffer buffer)
            {
                S2C_INSTANCE_13_36_16_NTC obj = new S2C_INSTANCE_13_36_16_NTC();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1List = ReadEntityList<CDataCommonPair<uint>>(buffer);
                return obj;
            }
        }
    }
}

