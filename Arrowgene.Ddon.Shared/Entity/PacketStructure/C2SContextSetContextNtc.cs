using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SContextSetContextNtc : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CONTEXT_SET_CONTEXT_NTC;

        public C2SContextSetContextNtc()
        {
            Base = new CDataContextSetBase();
            Additional = new CDataContextSetAdditional();
        }

        public CDataContextSetBase Base { get; set; }
        public CDataContextSetAdditional Additional { get; set; }

        public class Serializer : PacketEntitySerializer<C2SContextSetContextNtc>
        {
            public override void Write(IBuffer buffer, C2SContextSetContextNtc obj)
            {
                WriteEntity(buffer, obj.Base);
                WriteEntity(buffer, obj.Additional);
            }

            public override C2SContextSetContextNtc Read(IBuffer buffer)
            {
                C2SContextSetContextNtc obj = new C2SContextSetContextNtc();
                obj.Base = ReadEntity<CDataContextSetBase>(buffer);
                obj.Additional = ReadEntity<CDataContextSetAdditional>(buffer);
                return obj;
            }
        }
    }
}