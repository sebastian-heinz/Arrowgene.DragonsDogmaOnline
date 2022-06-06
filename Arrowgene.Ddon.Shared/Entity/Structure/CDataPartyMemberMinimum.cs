using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPartyMemberMinimum
    {
        public CDataPartyMemberMinimum()
        {
            CommunityCharacterBaseInfo = new CDataCommunityCharacterBaseInfo();
        }

        public CDataCommunityCharacterBaseInfo CommunityCharacterBaseInfo { get; set; }
        public byte MemberType { get; set; }
        public int MemberIndex { get; set; }
        public uint PawnId { get; set; }
        public bool IsLeader { get; set; }

        public class Serializer : EntitySerializer<CDataPartyMemberMinimum>
        {
            public override void Write(IBuffer buffer, CDataPartyMemberMinimum obj)
            {
                WriteEntity<CDataCommunityCharacterBaseInfo>(buffer, obj.CommunityCharacterBaseInfo);
                WriteByte(buffer, obj.MemberType);
                WriteInt32(buffer, obj.MemberIndex);
                WriteUInt32(buffer, obj.PawnId);
                WriteBool(buffer, obj.IsLeader);
            }

            public override CDataPartyMemberMinimum Read(IBuffer buffer)
            {
                CDataPartyMemberMinimum packet = new CDataPartyMemberMinimum();
                packet.CommunityCharacterBaseInfo = ReadEntity<CDataCommunityCharacterBaseInfo>(buffer);
                packet.MemberType = ReadByte(buffer);
                packet.MemberIndex = ReadInt32(buffer);
                packet.PawnId = ReadUInt32(buffer);
                packet.IsLeader = ReadBool(buffer);
                return packet;
            }
        }
    }
}