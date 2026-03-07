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

        public uint TargetContentId { get; set; }
        public uint UnknownFlag { get; set; }

        public class Serializer : PacketEntitySerializer<C2SBattleContentPartyMemberInfoReq>
        {
            public override void Write(IBuffer buffer, C2SBattleContentPartyMemberInfoReq obj)
            {
                WriteUInt32(buffer, obj.TargetContentId);
                WriteUInt32(buffer, obj.UnknownFlag);
            }

            public override C2SBattleContentPartyMemberInfoReq Read(IBuffer buffer)
            {
                C2SBattleContentPartyMemberInfoReq obj = new C2SBattleContentPartyMemberInfoReq();
                obj.TargetContentId = ReadUInt32(buffer);
                obj.UnknownFlag = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}


