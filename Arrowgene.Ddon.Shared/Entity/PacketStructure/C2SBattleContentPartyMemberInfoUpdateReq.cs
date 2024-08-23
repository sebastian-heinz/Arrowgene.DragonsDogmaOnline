using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBattleContentPartyMemberInfoUpdateReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BATTLE_CONTENT_PARTY_MEMBER_INFO_UPDATE_REQ;

        public C2SBattleContentPartyMemberInfoUpdateReq()
        {
        }

        public uint Unk0 { get; set; }
        public byte Unk1 { get; set; }
        public uint Unk2 { get; set; }

        public class Serializer : PacketEntitySerializer<C2SBattleContentPartyMemberInfoUpdateReq>
        {
            public override void Write(IBuffer buffer, C2SBattleContentPartyMemberInfoUpdateReq obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Unk2);
            }

            public override C2SBattleContentPartyMemberInfoUpdateReq Read(IBuffer buffer)
            {
                C2SBattleContentPartyMemberInfoUpdateReq obj = new C2SBattleContentPartyMemberInfoUpdateReq();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadByte(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}



