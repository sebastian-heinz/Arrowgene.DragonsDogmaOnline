using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillGetPawnLearnedAbilityListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_GET_PAWN_LEARNED_ABILITY_LIST_REQ;

        public uint PawnId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSkillGetPawnLearnedAbilityListReq>
        {
            public override void Write(IBuffer buffer, C2SSkillGetPawnLearnedAbilityListReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
            }

            public override C2SSkillGetPawnLearnedAbilityListReq Read(IBuffer buffer)
            {
                C2SSkillGetPawnLearnedAbilityListReq obj = new C2SSkillGetPawnLearnedAbilityListReq();
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}