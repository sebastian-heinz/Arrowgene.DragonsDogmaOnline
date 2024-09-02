using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentPartyMemberInfoRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BATTLE_CONTENT_PARTY_MEMBER_INFO_RES;

        public S2CBattleContentPartyMemberInfoRes()
        {
            Unk0 = new List<CDataBattleContentUnk6>();
        }

        public List<CDataBattleContentUnk6> Unk0 { get; set; }
        public bool Unk1 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBattleContentPartyMemberInfoRes>
        {
            public override void Write(IBuffer buffer, S2CBattleContentPartyMemberInfoRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.Unk0);
                WriteBool(buffer, obj.Unk1);
            }

            public override S2CBattleContentPartyMemberInfoRes Read(IBuffer buffer)
            {
                S2CBattleContentPartyMemberInfoRes obj = new S2CBattleContentPartyMemberInfoRes();
                ReadServerResponse(buffer, obj);
                obj.Unk0 = ReadEntityList<CDataBattleContentUnk6>(buffer);
                obj.Unk1 = ReadBool(buffer);
                return obj;
            }
        }
    }
}


