using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CContextSetContextBaseNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CONTEXT_SET_CONTEXT_BASE_NTC;

        public S2CContextSetContextBaseNtc()
        {
            Base = new CDataContextSetBase();
        }

        public CDataContextSetBase Base { get; set; }

        public class Serializer : PacketEntitySerializer<S2CContextSetContextBaseNtc>
        {
            public override void Write(IBuffer buffer, S2CContextSetContextBaseNtc obj)
            {
                WriteEntity<CDataContextSetBase>(buffer, obj.Base);
            }

            public override S2CContextSetContextBaseNtc Read(IBuffer buffer)
            {
                S2CContextSetContextBaseNtc obj = new S2CContextSetContextBaseNtc();
                obj.Base = ReadEntity<CDataContextSetBase>(buffer);
                return obj;
            }
        }
    }
}