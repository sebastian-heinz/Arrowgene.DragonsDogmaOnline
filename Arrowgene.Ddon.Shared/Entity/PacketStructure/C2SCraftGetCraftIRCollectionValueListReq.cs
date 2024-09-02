using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftGetCraftIRCollectionValueListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CRAFT_GET_CRAFT_IR_COLLECTION_VALUE_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SCraftGetCraftIRCollectionValueListReq>
        {
            public override void Write(IBuffer buffer, C2SCraftGetCraftIRCollectionValueListReq obj)
            {
            }

            public override C2SCraftGetCraftIRCollectionValueListReq Read(IBuffer buffer)
            {
                C2SCraftGetCraftIRCollectionValueListReq obj = new C2SCraftGetCraftIRCollectionValueListReq();
                return obj;
            }
        }

    }
}