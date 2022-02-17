using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CContext_35_14_16_Ntc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CONTEXT_35_14_16_NTC;
        
        public S2CContext_35_14_16_Ntc()
        {
            Unk0 = new List<CData_35_14_16>();
        }

        public List<CData_35_14_16> Unk0 { get; set; } // Probably a list of heartbeats for the different contexts?

        public class Serializer : EntitySerializer<S2CContext_35_14_16_Ntc>
        {
            static Serializer()
            {
                Id = PacketId.S2C_CONTEXT_35_14_16_NTC;
            }
            
            public override void Write(IBuffer buffer, S2CContext_35_14_16_Ntc obj)
            {
                WriteEntityList<CData_35_14_16>(buffer, obj.Unk0);
            }

            public override S2CContext_35_14_16_Ntc Read(IBuffer buffer)
            {
                S2CContext_35_14_16_Ntc obj = new S2CContext_35_14_16_Ntc();
                obj.Unk0 = ReadEntityList<CData_35_14_16>(buffer);
                return obj;
            }
        }
    }
}
