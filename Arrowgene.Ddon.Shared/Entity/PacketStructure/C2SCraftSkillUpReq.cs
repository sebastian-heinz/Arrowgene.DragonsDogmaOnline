using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftSkillUpReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CRAFT_CRAFT_SKILL_UP_REQ;

        public C2SCraftSkillUpReq()
        {
        }

        public uint PawnID { get; set; }
        public CraftSkillType SkillType { get; set; }
        public uint SkillLevel { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCraftSkillUpReq>
        {
            public override void Write(IBuffer buffer, C2SCraftSkillUpReq obj)
            {
                WriteUInt32(buffer, obj.PawnID);
                WriteUInt32(buffer, (uint)obj.SkillType);
                WriteUInt32(buffer, obj.SkillLevel);
            }

            public override C2SCraftSkillUpReq Read(IBuffer buffer)
            {
                C2SCraftSkillUpReq obj = new C2SCraftSkillUpReq();

                obj.PawnID = ReadUInt32(buffer);
                obj.SkillType = (CraftSkillType)ReadUInt32(buffer);
                obj.SkillLevel = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
