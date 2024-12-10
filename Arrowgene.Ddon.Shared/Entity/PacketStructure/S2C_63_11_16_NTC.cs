using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2C_63_11_16_NTC : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_63_11_16_NTC;

        public S2C_63_11_16_NTC()
        {
        }

        public uint StageNo {  get; set; }

        public class Serializer : PacketEntitySerializer<S2C_63_11_16_NTC>
        {
            public override void Write(IBuffer buffer, S2C_63_11_16_NTC obj)
            {
                WriteUInt32(buffer, obj.StageNo);
            }

            public override S2C_63_11_16_NTC Read(IBuffer buffer)
            {
                S2C_63_11_16_NTC obj = new S2C_63_11_16_NTC();
                obj.StageNo = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
