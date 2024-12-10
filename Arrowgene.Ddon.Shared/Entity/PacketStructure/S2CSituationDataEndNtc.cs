using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSituationDataEndNtc : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_63_1_16_NTC;

        public S2CSituationDataEndNtc()
        {
        }

        public uint Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSituationDataEndNtc>
        {
            public override void Write(IBuffer buffer, S2CSituationDataEndNtc obj)
            {
                WriteUInt32(buffer, obj.Unk0);
            }

            public override S2CSituationDataEndNtc Read(IBuffer buffer)
            {
                S2CSituationDataEndNtc obj = new S2CSituationDataEndNtc();
                obj.Unk0 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
