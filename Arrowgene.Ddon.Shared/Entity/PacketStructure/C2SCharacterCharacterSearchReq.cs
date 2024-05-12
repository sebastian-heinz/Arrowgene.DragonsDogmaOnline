using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterCharacterSearchReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHARACTER_CHARACTER_SEARCH_REQ;

        public CDataCharacterSearchParam SearchParam { get; set; }

        public C2SCharacterCharacterSearchReq()
        {
            SearchParam = new CDataCharacterSearchParam();
        }

        public class Serializer : PacketEntitySerializer<C2SCharacterCharacterSearchReq>
        {
            public override void Write(IBuffer buffer, C2SCharacterCharacterSearchReq obj)
            {
                WriteEntity<CDataCharacterSearchParam>(buffer, obj.SearchParam);
            }

            public override C2SCharacterCharacterSearchReq Read(IBuffer buffer)
            {
                C2SCharacterCharacterSearchReq obj = new C2SCharacterCharacterSearchReq();
                obj.SearchParam = ReadEntity<CDataCharacterSearchParam>(buffer);
                return obj;
            }
        }
    }
}
