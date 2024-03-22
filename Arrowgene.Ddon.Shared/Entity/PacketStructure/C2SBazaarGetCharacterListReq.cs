using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBazaarGetCharacterListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BAZAAR_GET_CHARACTER_LIST_REQ;

        public C2SBazaarGetCharacterListReq()
        {
            
        }

        

        public class Serializer : PacketEntitySerializer<C2SBazaarGetCharacterListReq>
        {
            public override void Write(IBuffer buffer, C2SBazaarGetCharacterListReq obj)
            {
                
            }

            public override C2SBazaarGetCharacterListReq Read(IBuffer buffer)
            {
                C2SBazaarGetCharacterListReq obj = new C2SBazaarGetCharacterListReq();
                
                return obj;
            }
        }

    }
}