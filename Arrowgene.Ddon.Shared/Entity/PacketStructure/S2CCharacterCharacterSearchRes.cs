using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterCharacterSearchRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CHARACTER_CHARACTER_SEARCH_RES;

        public List<CDataCharacterListElement> CharacterList { get; set; }

        public S2CCharacterCharacterSearchRes()
        {
            CharacterList = new List<CDataCharacterListElement>();
        }

        public class Serializer : PacketEntitySerializer<S2CCharacterCharacterSearchRes>
        {
            public override void Write(IBuffer buffer, S2CCharacterCharacterSearchRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataCharacterListElement>(buffer, obj.CharacterList);
            }

            public override S2CCharacterCharacterSearchRes Read(IBuffer buffer)
            {
                S2CCharacterCharacterSearchRes obj = new S2CCharacterCharacterSearchRes();
                ReadServerResponse(buffer, obj);
                obj.CharacterList = ReadEntityList<CDataCharacterListElement>(buffer);
                return obj;
            }
        }
    }
}
