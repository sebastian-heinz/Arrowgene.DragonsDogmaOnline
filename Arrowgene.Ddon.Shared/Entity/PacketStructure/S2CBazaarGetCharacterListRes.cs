using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBazaarGetCharacterListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BAZAAR_GET_CHARACTER_LIST_RES;

        public S2CBazaarGetCharacterListRes()
        {
            BazaarList = new List<CDataBazaarCharacterInfo>();
        }

        public List<CDataBazaarCharacterInfo> BazaarList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBazaarGetCharacterListRes>
        {
            public override void Write(IBuffer buffer, S2CBazaarGetCharacterListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataBazaarCharacterInfo>(buffer, obj.BazaarList);
            }

            public override S2CBazaarGetCharacterListRes Read(IBuffer buffer)
            {
                S2CBazaarGetCharacterListRes obj = new S2CBazaarGetCharacterListRes();
                ReadServerResponse(buffer, obj);
                obj.BazaarList = ReadEntityList<CDataBazaarCharacterInfo>(buffer);
                return obj;
            }
        }
    }
}