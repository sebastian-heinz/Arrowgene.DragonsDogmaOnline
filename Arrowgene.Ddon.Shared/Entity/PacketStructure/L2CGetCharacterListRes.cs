using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class L2CGetCharacterListRes : ServerResponse
    {
        public L2CGetCharacterListRes()
        {
            CharacterList = new();
        }

        public override PacketId Id => PacketId.L2C_GET_CHARACTER_LIST_RES;

        public List<CDataCharacterListInfo> CharacterList { get; set; }

        public class Serializer : PacketEntitySerializer<L2CGetCharacterListRes>
        {
            public override void Write(IBuffer buffer, L2CGetCharacterListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.CharacterList);
            }

            public override L2CGetCharacterListRes Read(IBuffer buffer)
            {
                L2CGetCharacterListRes obj = new L2CGetCharacterListRes();
                ReadServerResponse(buffer, obj);
                obj.CharacterList = ReadEntityList<CDataCharacterListInfo>(buffer);
                return obj;
            }
        }
    }
}
