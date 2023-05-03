using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillSetOffPawnAbilityReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_SET_OFF_PAWN_ABILITY_REQ;

        public uint PawnId { get; set; }
        public byte SlotNo { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSkillSetOffPawnAbilityReq>
        {
            public override void Write(IBuffer buffer, C2SSkillSetOffPawnAbilityReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, obj.SlotNo);
            }

            public override C2SSkillSetOffPawnAbilityReq Read(IBuffer buffer)
            {
                C2SSkillSetOffPawnAbilityReq obj = new C2SSkillSetOffPawnAbilityReq();
                obj.PawnId = ReadUInt32(buffer);
                obj.SlotNo = ReadByte(buffer);
                return obj;
            }
        }

    }
}