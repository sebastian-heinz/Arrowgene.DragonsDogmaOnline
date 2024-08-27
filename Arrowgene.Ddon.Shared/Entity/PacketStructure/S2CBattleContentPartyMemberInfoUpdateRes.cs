using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentPartyMemberInfoUpdateRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BATTLE_CONTENT_PARTY_MEMBER_INFO_UPDATE_RES;

        public S2CBattleContentPartyMemberInfoUpdateRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CBattleContentPartyMemberInfoUpdateRes>
        {
            public override void Write(IBuffer buffer, S2CBattleContentPartyMemberInfoUpdateRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CBattleContentPartyMemberInfoUpdateRes Read(IBuffer buffer)
            {
                S2CBattleContentPartyMemberInfoUpdateRes obj = new S2CBattleContentPartyMemberInfoUpdateRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
