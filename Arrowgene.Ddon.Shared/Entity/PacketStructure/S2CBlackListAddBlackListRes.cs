using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBlackListAddBlackListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BLACK_LIST_ADD_BLACK_LIST_RES;

        public CDataCommunityCharacterBaseInfo CharacterBaseInfo { get; set; } = new();

        public class Serializer : PacketEntitySerializer<S2CBlackListAddBlackListRes>
        {
            public override void Write(IBuffer buffer, S2CBlackListAddBlackListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity(buffer, obj.CharacterBaseInfo);
            }

            public override S2CBlackListAddBlackListRes Read(IBuffer buffer)
            {
                S2CBlackListAddBlackListRes obj = new S2CBlackListAddBlackListRes();
                ReadServerResponse(buffer, obj);
                obj.CharacterBaseInfo = ReadEntity<CDataCommunityCharacterBaseInfo>(buffer);
                return obj;
            }
        }
    }
}
