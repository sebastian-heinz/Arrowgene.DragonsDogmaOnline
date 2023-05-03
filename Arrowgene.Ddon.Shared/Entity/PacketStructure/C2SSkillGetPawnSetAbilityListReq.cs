using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillGetPawnSetAbilityListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_GET_PAWN_SET_ABILITY_LIST_REQ;

        public uint PawnId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSkillGetPawnSetAbilityListReq>
        {
            public override void Write(IBuffer buffer, C2SSkillGetPawnSetAbilityListReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
            }

            public override C2SSkillGetPawnSetAbilityListReq Read(IBuffer buffer)
            {
                C2SSkillGetPawnSetAbilityListReq obj = new C2SSkillGetPawnSetAbilityListReq();
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}