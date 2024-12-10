using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanGetFurnitureReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_GET_FURNITURE_REQ;

        public C2SClanGetFurnitureReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SClanGetFurnitureReq>
        {
            public override void Write(IBuffer buffer, C2SClanGetFurnitureReq obj)
            {
            }

            public override C2SClanGetFurnitureReq Read(IBuffer buffer)
            {
                C2SClanGetFurnitureReq obj = new C2SClanGetFurnitureReq();
                return obj;
            }
        }
    }
}
