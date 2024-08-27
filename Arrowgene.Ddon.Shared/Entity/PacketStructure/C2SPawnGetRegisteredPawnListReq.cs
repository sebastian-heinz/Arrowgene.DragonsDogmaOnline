using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnGetRegisteredPawnListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_GET_REGISTERED_PAWN_LIST_REQ;

        public C2SPawnGetRegisteredPawnListReq()
        {
            SearchParam = new CDataPawnSearchParameter();
        }

        public CDataPawnSearchParameter SearchParam;

        public class Serializer : PacketEntitySerializer<C2SPawnGetRegisteredPawnListReq>
        {
            public override void Write(IBuffer buffer, C2SPawnGetRegisteredPawnListReq obj)
            {
                WriteEntity(buffer, obj.SearchParam);   
            }

            public override C2SPawnGetRegisteredPawnListReq Read(IBuffer buffer)
            {
                C2SPawnGetRegisteredPawnListReq obj = new C2SPawnGetRegisteredPawnListReq();
                obj.SearchParam = ReadEntity<CDataPawnSearchParameter>(buffer);
                return obj;
            }
        }
    }
}
