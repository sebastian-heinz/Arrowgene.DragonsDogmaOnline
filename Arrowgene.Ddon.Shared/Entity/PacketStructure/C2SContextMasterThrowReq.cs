using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SContextMasterThrowReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CONTEXT_MASTER_THROW_REQ;

        public C2SContextMasterThrowReq()
        {
            Info = new List<CDataMasterInfo>();
        }

        public List<CDataMasterInfo> Info { get; set; }

        public class Serializer : PacketEntitySerializer<C2SContextMasterThrowReq>
        {
            public override void Write(IBuffer buffer, C2SContextMasterThrowReq obj)
            {
                WriteEntityList<CDataMasterInfo>(buffer, obj.Info);
            }

            public override C2SContextMasterThrowReq Read(IBuffer buffer)
            {
                C2SContextMasterThrowReq obj = new C2SContextMasterThrowReq();
                obj.Info = ReadEntityList<CDataMasterInfo>(buffer);
                return obj;
            }
        }

    }
}