using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJob24_5_16_NTC : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_JOB_24_5_16_NTC;

        public S2CJob24_5_16_NTC()
        {
            Unk0List = new List<CDataJobMasterTargetData>();
        }

        public List<CDataJobMasterTargetData> Unk0List { get; set; }

        public class Serializer : PacketEntitySerializer<S2CJob24_5_16_NTC>
        {
            public override void Write(IBuffer buffer, S2CJob24_5_16_NTC obj)
            {
                WriteEntityList(buffer, obj.Unk0List);
            }

            public override S2CJob24_5_16_NTC Read(IBuffer buffer)
            {
                S2CJob24_5_16_NTC obj = new S2CJob24_5_16_NTC();
                obj.Unk0List = ReadEntityList<CDataJobMasterTargetData>(buffer);
                return obj;
            }
        }
    }
}
