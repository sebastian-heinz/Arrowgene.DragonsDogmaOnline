using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMandragoraGetSpeciesCategoryListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MANDRAGORA_GET_SPECIES_CATEGORY_LIST_REQ;

        public C2SMandragoraGetSpeciesCategoryListReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SMandragoraGetSpeciesCategoryListReq>
        {
            public override void Write(IBuffer buffer, C2SMandragoraGetSpeciesCategoryListReq obj)
            {
            }

            public override C2SMandragoraGetSpeciesCategoryListReq Read(IBuffer buffer)
            {
                C2SMandragoraGetSpeciesCategoryListReq obj = new C2SMandragoraGetSpeciesCategoryListReq();
                return obj;
            }
        }
    }
}
