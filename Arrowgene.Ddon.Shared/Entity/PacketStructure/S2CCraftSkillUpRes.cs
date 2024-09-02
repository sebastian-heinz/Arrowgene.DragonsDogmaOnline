using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCraftSkillUpRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CRAFT_CRAFT_SKILL_UP_RES;

        public uint PawnID { get; set; }
        public CraftSkillType SkillType { get; set; }
        public uint SkillLevel { get; set; }
        public uint UseCraftPoint { get; set; }
        public uint RemainCraftPoint { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCraftSkillUpRes>
        {
            public override void Write(IBuffer buffer, S2CCraftSkillUpRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteUInt32(buffer, obj.PawnID);
                WriteUInt32(buffer, (uint)obj.SkillType);
                WriteUInt32(buffer, obj.SkillLevel);
                WriteUInt32(buffer, obj.UseCraftPoint);
                WriteUInt32(buffer, obj.RemainCraftPoint);
            }

            public override S2CCraftSkillUpRes Read(IBuffer buffer)
            {
                S2CCraftSkillUpRes obj = new S2CCraftSkillUpRes();

                ReadServerResponse(buffer, obj);

                obj.PawnID = ReadUInt32(buffer);
                obj.SkillType = (CraftSkillType)ReadUInt32(buffer);
                obj.SkillLevel = ReadUInt32(buffer);
                obj.UseCraftPoint = ReadUInt32(buffer);
                obj.RemainCraftPoint = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
