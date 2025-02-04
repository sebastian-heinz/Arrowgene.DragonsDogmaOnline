using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2LGetCharacterListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2L_GET_CHARACTER_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2LGetCharacterListReq>
        {

            public override void Write(IBuffer buffer, C2LGetCharacterListReq obj)
            {
            }

            public override C2LGetCharacterListReq Read(IBuffer buffer)
            {
                C2LGetCharacterListReq obj = new C2LGetCharacterListReq();
                return obj;
            }
        }
    }
}
