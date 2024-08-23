using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBattleContentPartyMemberInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BATTLE_CONTENT_PARTY_MEMBER_INFO_REQ;

        public C2SBattleContentPartyMemberInfoReq()
        {
        }

        public uint Unk0 { get; set; }
        public uint Unk1 { get; set; }

        public class Serializer : PacketEntitySerializer<C2SBattleContentPartyMemberInfoReq>
        {
            public override void Write(IBuffer buffer, C2SBattleContentPartyMemberInfoReq obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
            }

            public override C2SBattleContentPartyMemberInfoReq Read(IBuffer buffer)
            {
                C2SBattleContentPartyMemberInfoReq obj = new C2SBattleContentPartyMemberInfoReq();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}


