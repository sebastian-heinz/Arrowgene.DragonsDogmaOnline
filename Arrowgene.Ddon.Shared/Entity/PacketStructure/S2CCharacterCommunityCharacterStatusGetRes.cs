using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterCommunityCharacterStatusGetRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CHARACTER_COMMUNITY_CHARACTER_STATUS_GET_RES;

        public class Serializer : PacketEntitySerializer<S2CCharacterCommunityCharacterStatusGetRes>
        {
            public override void Write(IBuffer buffer, S2CCharacterCommunityCharacterStatusGetRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CCharacterCommunityCharacterStatusGetRes Read(IBuffer buffer)
            {
                S2CCharacterCommunityCharacterStatusGetRes obj = new S2CCharacterCommunityCharacterStatusGetRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
            
        }
    }
}

