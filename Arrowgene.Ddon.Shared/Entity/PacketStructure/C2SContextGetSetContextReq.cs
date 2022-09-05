using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SContextGetSetContextReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CONTEXT_GET_SET_CONTEXT_REQ;

        public C2SContextGetSetContextReq()
        {
            Base = new CDataContextSetBase();
        }

        public CDataContextSetBase Base { get; set; }

        public class Serializer : PacketEntitySerializer<C2SContextGetSetContextReq>
        {            

            public override void Write(IBuffer buffer, C2SContextGetSetContextReq obj)
            {
                WriteEntity<CDataContextSetBase>(buffer, obj.Base);
            }

            public override C2SContextGetSetContextReq Read(IBuffer buffer)
            {
                C2SContextGetSetContextReq obj = new C2SContextGetSetContextReq();
                obj.Base = ReadEntity<CDataContextSetBase>(buffer);
                return obj;
            }
        }

    }
}
