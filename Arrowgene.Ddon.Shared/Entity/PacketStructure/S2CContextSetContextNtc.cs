using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CContextSetContextNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CONTEXT_SET_CONTEXT_NTC;

        public S2CContextSetContextNtc()
        {
            Base = new CDataContextSetBase();
            Additional = new CDataContextSetAdditional();
        }

        public CDataContextSetBase Base { get; set; }
        public CDataContextSetAdditional Additional { get; set; }

        public class Serializer : PacketEntitySerializer<S2CContextSetContextNtc>
        {
            public override void Write(IBuffer buffer, S2CContextSetContextNtc obj)
            {
                WriteEntity(buffer, obj.Base);
                WriteEntity(buffer, obj.Additional);
            }

            public override S2CContextSetContextNtc Read(IBuffer buffer)
            {
                S2CContextSetContextNtc obj = new S2CContextSetContextNtc();
                obj.Base = ReadEntity<CDataContextSetBase>(buffer);
                obj.Additional = ReadEntity<CDataContextSetAdditional>(buffer);
                return obj;
            }
        }
    }
}